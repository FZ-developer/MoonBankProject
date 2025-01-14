﻿using Common.DTO;
using Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;

namespace MoonBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RewardsController : ControllerBase
    {
        private readonly IRewardService _rewardService;

        public RewardsController(IRewardService rewardService)
        {
            _rewardService = rewardService;
        }

        [HttpGet("RewardsList")]
        public ActionResult<ResponseDTO> GetAllRewards()
        {
            var response = _rewardService.GetAllRewards();

            return Ok(response);
        }

        [HttpGet("{idReward}")]
        public ActionResult<ResponseDTO> GetRewardById(int idReward)
        {
            var response = _rewardService.GetRewardById(idReward);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }
        }
    }
}
