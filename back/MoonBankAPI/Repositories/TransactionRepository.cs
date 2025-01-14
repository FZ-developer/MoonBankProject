﻿using Common.DTO;
using Common.Exceptions;
using Contracts.Repositories;
using DataAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private MoonBankContext _context { get; set; }
        public TransactionRepository(MoonBankContext context)
        {
            _context = context;
        }

        public IList<TransactionDTO> GetTransactionsHistory(string CBU_CVU)
        {
            var sourceAccount = _context.Accounts.FirstOrDefault(a => a.CBU_CVU == CBU_CVU);
            if (sourceAccount == null)
            {
                throw new TransactionExceptions("No transactions found for the specified CBU_CVU or it does not exists.");
            }
            var idAccount = sourceAccount.IdAccount;
            var transactions = _context.Transactions
                .Where(x => x.IdSourceAccount == idAccount || x.IdDestinationAccount == idAccount)
                .Select(x => new TransactionDTO
                {
                    IdTransaction = x.IdTransaction,
                    TypeTransaction = (Common.DTO.TypeTransaction)x.TypeTransaction,
                    TypeDeposit = (Common.DTO.TypeDeposit)x.TypeDeposit,
                    Date = x.Date,
                    Amount = x.Amount,
                    IdSourceAccount = x.IdSourceAccount,
                    IdDestinationAccount = x.IdDestinationAccount,
                    IdService= x.IdService,
                    IdReward = x.IdReward
                })
                .ToList();

            if (transactions.Count == 0)
            {
                throw new TransactionExceptions("No transactions found for the specified account.");
            }

            return transactions;
        }


        public void MakeTransaction(TransactionDTO transactionDTO)
        {
            var option = transactionDTO.TypeTransaction;

            switch (option)
            {
                case Common.DTO.TypeTransaction.Deposit:

                    MakeDeposit(transactionDTO);

                    break;

                case Common.DTO.TypeTransaction.PayService:

                    PayService(transactionDTO);

                    break;

                case Common.DTO.TypeTransaction.Transfer:

                    MakeTransfer(transactionDTO);

                    break;

                case Common.DTO.TypeTransaction.Reward:

                    RedeemReward(transactionDTO);

                    break;

                default:

                    throw new TransactionExceptions("Invalid transaction type.");
            }
        }

        public void MakeDeposit(TransactionDTO transactionDTO) // hacer estos metodos internos privados 
        {
            
            if (transactionDTO.Amount > 50000)
            {
                throw new TransactionExceptions("The amount entered is greater than $50000, which is the allowed limit.");
            }

            if (transactionDTO.Amount < 0)
            {
                throw new TransactionExceptions("You can not deposit a negative amount.");
            }

            var destinationAccount = _context.Accounts.FirstOrDefault(a => a.CBU_CVU == transactionDTO.DestinationAccountCBU_CVU || a.Alias == transactionDTO.DestinationAccountAlias);

            if (destinationAccount == null)
            {
                throw new TransactionExceptions("Destination Account not found.");
            }

           

            destinationAccount.Balance += transactionDTO.Amount;

            var transaction = new DataAccess.Models.Transaction
            {
                IdDestinationAccount = destinationAccount.IdAccount,
                TypeTransaction = DataAccess.Models.TypeTransaction.Deposit,
                TypeDeposit = (DataAccess.Models.TypeDeposit?)transactionDTO.TypeDeposit,
                Amount = transactionDTO.Amount,
                Date = DateTime.Now
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges();            
        }

        public void PayService(TransactionDTO transactionDTO)
        {

            var sourceAccount = _context.Accounts.FirstOrDefault(a => a.CBU_CVU == transactionDTO.DestinationAccountCBU_CVU);
            var service = _context.Services.FirstOrDefault(s => s.IdService == transactionDTO.IdService);

            if (sourceAccount == null)
            {
                throw new TransactionExceptions("Account not found.");
            }

            if (service == null)
            {
                throw new TransactionExceptions("Service not found.");
            }

            if (service.Amount > sourceAccount.Balance)
            {
                throw new TransactionExceptions("You do not have enough balance to pay for this service.");                
            }

            sourceAccount.Balance -= service.Amount;
            sourceAccount.RewardPoints += 50;

            var transaction = new DataAccess.Models.Transaction
            {
                IdSourceAccount = sourceAccount.IdAccount,
                IdService = service.IdService,
                TypeTransaction = DataAccess.Models.TypeTransaction.PayService,
                Amount = service.Amount,
                Date = DateTime.Now
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }

        public void MakeTransfer(TransactionDTO transactionDTO)
        {
            if (transactionDTO.Amount > 50000)
            {
                throw new TransactionExceptions("Entered amount exceeds the allowed limit.");
            }

            if (transactionDTO.Amount < 0)
            {
                throw new TransactionExceptions("You can not transfer a negative amount.");
            }

            var sourceAccount = _context.Accounts.FirstOrDefault(a => a.CBU_CVU == transactionDTO.SourceAccountCBU_CVU);
                        
            if (sourceAccount.CBU_CVU == transactionDTO.DestinationAccountCBU_CVU || sourceAccount.Alias == transactionDTO.DestinationAccountAlias)
            {
                throw new TransactionExceptions("Cannot transfer to the same account.");
            }

            var destinationAccount = _context.Accounts.FirstOrDefault(a => a.CBU_CVU == transactionDTO.DestinationAccountCBU_CVU || a.Alias == transactionDTO.DestinationAccountAlias);


            if (destinationAccount == null)
            {
                throw new TransactionExceptions("CBU_CVU or alias is not associated with any account");
            }

            if (transactionDTO.Amount > sourceAccount.Balance)
            {
                throw new TransactionExceptions("Insufficient balance.");
            }

            sourceAccount.Balance -= transactionDTO.Amount;
            destinationAccount.Balance += transactionDTO.Amount;
            sourceAccount.RewardPoints += 100;

            var transaction = new DataAccess.Models.Transaction
            {
                TypeTransaction = DataAccess.Models.TypeTransaction.Transfer,
                IdSourceAccount = sourceAccount.IdAccount,
                IdDestinationAccount = destinationAccount.IdAccount,
                Amount = transactionDTO.Amount,
                TypeDeposit= (DataAccess.Models.TypeDeposit?)transactionDTO.TypeDeposit,
                Date = DateTime.Now
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }

        public void RedeemReward(TransactionDTO transactionDTO)
        {
            var sourceAccount = _context.Accounts.FirstOrDefault(a => a.Alias == transactionDTO.DestinationAccountAlias);
            var reward = _context.Rewards.FirstOrDefault(r => r.IdReward == transactionDTO.IdReward);

            if (sourceAccount == null)
            {
                throw new TransactionExceptions("Account not found.");
            }

            if (reward == null)
            {
                throw new TransactionExceptions("Reward not found.");
            }

            if (reward.Points > sourceAccount.RewardPoints)
            {
                throw new TransactionExceptions("You do not have enough points to redeem this reward.");
            }

            sourceAccount.RewardPoints -= reward.Points;

            var transaction = new DataAccess.Models.Transaction
            {
                IdSourceAccount = sourceAccount.IdAccount,
                TypeTransaction = DataAccess.Models.TypeTransaction.Reward,
                Amount = 0,
                IdReward = reward.IdReward,
                Date = DateTime.Now
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }







    }
}
