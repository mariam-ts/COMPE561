using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csrecap
{
    class Account
    {
        public double AccountBalance { get; protected set; }
        public string AccountName { get; private set; }
        public int AccountNum { get; private set; }

        public Account(int accountBalance, string accountName, int accountNum)
        {
            AccountBalance = accountBalance;
            AccountName = accountName;
            AccountNum = accountNum;
        }

        public void Menu()
        {
            Console.WriteLine("Hi!");
            
            int userChoice = 0;
            do {
                Console.WriteLine("************");
                Console.WriteLine("Menu");
                Console.WriteLine("************");
                Console.WriteLine("Enter your command");
                Console.WriteLine("1. Check Balance");
                Console.WriteLine("2. Deposit Amount");
                Console.WriteLine("3. Withdraw Amount");
                Console.WriteLine("4. Exit");
                try
                {
                    userChoice = int.Parse(Console.ReadLine());
                    switch (userChoice)
                    {
                        case 1:
                            CheckBalance();
                            break;
                        case 2:
                            DepositAmount();
                            break;
                        case 3:
                            WithdrawAmount();
                            break;
                        case 4:
                            Console.WriteLine("Thank you for using our service");
                            break;
                        default:
                            Console.WriteLine("invalid option");
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter a valid number");
                }
            } while (userChoice != 4);
        }

        protected void CheckBalance()
        {
            Console.WriteLine($"The balance of the account {AccountName} is ${AccountBalance}");
        }

        protected virtual void DepositAmount()
        {
            Console.WriteLine("Please, enter the number you'd like to deposit");
            int amount = int.Parse(Console.ReadLine());
            if(amount >= 0)
            {
                AccountBalance += amount;
                Console.WriteLine($"${amount} was deposited to the account {AccountName}");
                CheckBalance();
            } else
            {
                Console.WriteLine("Please enter a positive number to deposit");
                DepositAmount();
            }
        }

        protected virtual void WithdrawAmount()
        {
            Console.WriteLine("Please, enter the number you'd like to withdraw");
            int amount = int.Parse(Console.ReadLine());
            if (amount < 0)
            {
                Console.WriteLine("Please enter a positive number to withdraw");
                WithdrawAmount();
            } else if (amount > AccountBalance)
            {
                Console.WriteLine("You do not have enough money to withdraw this amount");
                WithdrawAmount();
            } else {
                AccountBalance -= amount;
                Console.WriteLine($"${amount} was withdrawn from the account {AccountName}");
                CheckBalance();
            }
        }
    }

   

    class InterestAccount : Account
    {
        protected double InterestRate { get;  }
        protected double InterestLimit { get; }
        protected double OverDraftLimit { get;  }

        private double OverDraft { get; set; }
        private DateTime OverDraftDate { get; set; }

        private double PositiveBalance { get; set; }
        private DateTime PositiveBalanceDate { get; set; }
        private Boolean AccountIsBlocked = false;
        private Boolean PositiveBalanceForMonth = false;
       

        public InterestAccount(int accountBalance, string accountName, int accountNum, double InterestRate, double InterestLimit, double OverDraftLimit)
            :base(accountBalance, accountName, accountNum)
        {
            this.InterestRate = InterestRate;
            this.InterestRate = InterestRate;
            this.OverDraftLimit = OverDraftLimit;
        }


        private void CheckAccount()
        {
            this.AccountIsBlocked = (DateTime.Now - OverDraftDate).TotalDays > 30;
            
        }
        private void checkPositiveAccount()
        {
            this.PositiveBalanceForMonth = (DateTime.Now - this.PositiveBalanceDate).TotalDays > 30;

     
        }
        protected override void DepositAmount()
        {
            CheckAccount();
  
            if (OverDraft>0 && this.AccountIsBlocked)
            {
                Console.WriteLine("Your Accoount is Blocked");
                return;
            }

            //add interest according deposit amount
            Console.WriteLine("Enter deposit");
            double deposit = double.Parse(Console.ReadLine());
            if (deposit >= 0)
            {
                if (this.OverDraft >= deposit)
                {
                    this.OverDraft -= deposit;
                    this.AccountBalance = 0;
                    this.PositiveBalance = 0;
                    this.PositiveBalanceDate = DateTime.Now;
                }
                else
                {
                    this.AccountBalance += (deposit - this.OverDraft);
                    this.OverDraft = 0;
                    this.PositiveBalance = this.AccountBalance;
                    this.PositiveBalanceDate = DateTime.Now;

                }

                Console.WriteLine($"${deposit} was deposited to the account {AccountName}");
                CheckBalance();
            }
            else
            {
                Console.WriteLine("Please enter a positive number to deposit");
                DepositAmount();
            }
            deposit += AddInterest(deposit);

            checkPositiveAccount();
            if (this.PositiveBalanceForMonth)
            {
                if (this.PositiveBalance > 0)
                {
                    this.AccountBalance += AddInterest(PositiveBalance);

                }
                this.PositiveBalanceDate = DateTime.Now;
                this.PositiveBalance = 0;
            }




        }
        protected double AddInterest(double deposit)
        {
            if (deposit > 1000)
            {
                double interest = deposit * this.InterestRate / 100;

                if (interest < this.InterestLimit)
                {
                    return interest;
                }
                else
                {
                    return (double)this.InterestLimit;
                }
            }
            else
            {
                return 0;
            }

        }
        private void takeOverDraft(double temp)
        {
            Console.WriteLine($"You had over draft: {temp} now you have: {this.OverDraft} ");
        }
        protected override void WithdrawAmount()
        {
            this.CheckAccount();
            if (OverDraft > 0 && this.AccountIsBlocked)
            {
                Console.WriteLine("Your Accoount is Blocked");
                return;
            }

            Console.WriteLine("Enter money to withdraw");
            double withdraw = double.Parse(Console.ReadLine());

            if (withdraw <= 0)
            {
                Console.WriteLine("Amount should be positive number");
            }
            else if (withdraw>this.AccountBalance)
            {
                if(withdraw + OverDraft < this.OverDraftLimit)
                {
                    var temp = this.OverDraft;
                    this.OverDraft += (withdraw-this.AccountBalance);
                    this.OverDraftDate = DateTime.Now;
                    this.PositiveBalanceDate = DateTime.Now;
                    this.PositiveBalance = 0;
                    this.AccountBalance = 0;
                    takeOverDraft(temp);
                    CheckBalance();
                }
                else
                {
                    Console.WriteLine("To Withdraw money is impossible, Please try again");
                }
            }
            else if(withdraw== this.AccountBalance)
            {
                this.AccountBalance = 0;
                this.PositiveBalance = 0;
                this.PositiveBalanceDate = DateTime.Now;
                Console.WriteLine($"You have withdrawn {withdraw}$");
                CheckBalance();
                
            }
            else
            {
                this.AccountBalance -= withdraw;
                this.PositiveBalance = this.AccountBalance;
                this.PositiveBalanceDate = DateTime.Now;
                Console.WriteLine($"You have withdrawn {withdraw}$");
                CheckBalance();
            }
            
        }
    }


    class VipAccount : InterestAccount
    {
        

        private double OverDraft { get; set; }
        private DateTime OverDraftDate { get; set; }

        private double PositiveBalance { get; set; }
        private DateTime PositiveBalanceDate { get; set; }
        private Boolean AccountIsBlocked = false;
        private Boolean PositiveBalanceForMonth = false;

        public VipAccount(int accountBalance, string accountName, int accountNum, double InterestRate, double InterestLimit, double OverDraftLimit)
            : base(accountBalance, accountName, accountNum, InterestRate = 0.2, InterestLimit = 10, OverDraftLimit = 5000)
        {
        }

        

    }
    class BusinessAccount : InterestAccount
    {


        private double OverDraft { get; set; }
        private DateTime OverDraftDate { get; set; }

        private double PositiveBalance { get; set; }
        private DateTime PositiveBalanceDate { get; set; }
        private Boolean AccountIsBlocked = false;
        private Boolean PositiveBalanceForMonth = false;

        public BusinessAccount(int accountBalance, string accountName, int accountNum, double InterestRate, double InterestLimit, double OverDraftLimit)
            : base(accountBalance, accountName, accountNum, InterestRate = 0.1, InterestLimit = 5, OverDraftLimit = 3000)
        {
        }



    }
}


