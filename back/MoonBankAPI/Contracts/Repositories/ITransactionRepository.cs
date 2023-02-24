﻿using Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Contracts.Repositories
{
    public interface ITransactionRepository
    {
        IList<TransactionDTO> GetTransactionsHistory(int idAccount);

        void MakeTransaction(TransactionDTO transactionDTO);
    }
}
