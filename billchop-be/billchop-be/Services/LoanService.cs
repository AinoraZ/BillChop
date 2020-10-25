﻿using BillChopBE.DataAccessLayer.Filters;
using BillChopBE.DataAccessLayer.Filters.Factories;
using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillChopBE.Services
{
    public interface ILoanService
    {
        Task<IList<Loan>> GetProvidedLoans(Guid loanerId, Guid? groupId);
        Task<IList<Loan>> GetReceivedLoans(Guid loaneeId, Guid? groupId);
        Task<IList<Loan>> GetSelfLoans(Guid loanerAndLoaneeId, Guid? groupId);
        Task<IList<Loan>> GetFilteredLoans(LoanFilterInfo loanFilterInfo);
    }

    public class LoanService : ILoanService
    {
        private readonly ILoanRepository loanRepository;
        private readonly ILoanDbFilterFactory loanDbFilterFactory;

        public LoanService(ILoanRepository loanRepository, ILoanDbFilterFactory loanDbFilterFactory)
        {
            this.loanRepository = loanRepository;
            this.loanDbFilterFactory = loanDbFilterFactory;
        }

        public async Task<IList<Loan>> GetProvidedLoans(Guid loanerId, Guid? groupId)
        {
            var filterInfo = new LoanFilterInfo()
            {
                LoanerId = loanerId,
                GroupId = groupId,
            };

            return await GetFilteredLoans(filterInfo);
        }

        public async Task<IList<Loan>> GetReceivedLoans(Guid loaneeId, Guid? groupId)
        {
            var filterInfo = new LoanFilterInfo()
            {
                LoaneeId = loaneeId,
                GroupId = groupId,
            };

            return await GetFilteredLoans(filterInfo);
        }

        public async Task<IList<Loan>> GetSelfLoans(Guid loanerAndLoaneeId, Guid? groupId)
        {
            var filterInfo = new LoanFilterInfo()
            {
                LoaneeId = loanerAndLoaneeId,
                LoanerId = loanerAndLoaneeId,
                GroupId = groupId,
            };

            return await GetFilteredLoans(filterInfo);
        }

        public Task<IList<Loan>> GetFilteredLoans(LoanFilterInfo loanFilterInfo)
        {
            var filter = loanDbFilterFactory.Create(loanFilterInfo);
            return loanRepository.GetAllAsync(filter);
        }
    }
}
