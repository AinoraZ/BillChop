﻿using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using BillChopBE.Exceptions;
using BillChopBE.Extensions;
using BillChopBE.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace BillChopBE.Services
{
    public interface IBillService
    {
        Task<Bill> CreateAndSplitBillAsync(CreateNewBill newBill);
        Task<IList<Bill>> GetBillsAsync();
        Task<IList<Bill>> GetGroupBillsAsync(Guid groupId);
    }

    public class BillService : IBillService
    {
        private readonly IBillRepository billRepository;
        private readonly IExpenseRepository expenseRepository;
        private readonly IGroupRepository groupRepository;
        private readonly IUserRepository userRepository;

        public BillService(IBillRepository billRepository,
            IExpenseRepository expenseRepository,
            IGroupRepository groupRepository,
            IUserRepository userRepository)
        {
            this.billRepository = billRepository;
            this.expenseRepository = expenseRepository;
            this.groupRepository = groupRepository;
            this.userRepository = userRepository;
        }

        public Task<IList<Bill>> GetBillsAsync()
        {
            return billRepository.GetAllAsync();
        }

        public async Task<IList<Bill>> GetGroupBillsAsync(Guid groupId)
        {
            var group = await groupRepository.GetByIdAsync(groupId);
            if (group == null)
                throw new NotFoundException($"Group with id {groupId} does not exist");

            return await billRepository.GetBillsByGroupId(groupId);
        }

        public async Task<Bill> CreateAndSplitBillAsync(CreateNewBill newBill)
        {
            newBill.Validate();

            var group = await groupRepository.GetByIdAsync(newBill.GroupContextId);
            if (group == null)
                throw new NotFoundException($"Group with id {newBill.GroupContextId} does not exist.");

            var loaner = await userRepository.GetByIdAsync(newBill.LoanerId);
            if (loaner == null)
                throw new NotFoundException($"Payee with id {newBill.LoanerId} does not exist.");

            var bill = new Bill()
            {
                Name = newBill.Name,
                Total = newBill.Total,
                Loaner = loaner,
                GroupContext = group,
            };

            bill = await billRepository.AddAsync(bill);
            await SplitBillAsync(bill);

            return bill;
        }

        private async Task<IEnumerable<Expense>> SplitBillAsync(Bill bill)
        {
            var payingUsers = bill.GroupContext.Users.ToList();
            var amounts = bill.Total.SplitEqually(payingUsers.Count).ToList();

            var expenses = payingUsers
                .Select((user, index) => new Expense()
                {
                    Bill = bill,
                    Loanee = user,
                    Amount = amounts[index]
                });

            foreach (var expense in expenses) 
            {
                await expenseRepository.AddAsync(expense);
            }

            return expenses;
        }
    }
}
