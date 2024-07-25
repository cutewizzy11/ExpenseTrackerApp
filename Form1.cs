using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExpenseTrackerApp
{
    public partial class Form1 : Form
    {
        private const string FilePath = "expenses.txt";
        private List<Expense> expenses = new List<Expense>();

        public Form1()
        {
            InitializeComponent();
            LoadExpenses();
            UpdateTotalAmount();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string description = textBoxDescription.Text.Trim();
            if (decimal.TryParse(textBoxAmount.Text.Trim(), out decimal amount) && amount > 0)
            {
                DateTime date = dateTimePickerDate.Value;

                Expense newExpense = new Expense(description, amount, date);
                expenses.Add(newExpense);
                listBoxExpenses.Items.Add(newExpense);
                ClearInputFields();
                UpdateTotalAmount();
            }
            else
            {
                MessageBox.Show("Please enter a valid amount.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (listBoxExpenses.SelectedIndex >= 0)
            {
                Expense selectedExpense = (Expense)listBoxExpenses.SelectedItem;
                selectedExpense.Description = textBoxDescription.Text.Trim();
                if (decimal.TryParse(textBoxAmount.Text.Trim(), out decimal amount) && amount > 0)
                {
                    selectedExpense.Amount = amount;
                    selectedExpense.Date = dateTimePickerDate.Value;

                    listBoxExpenses.Items[listBoxExpenses.SelectedIndex] = selectedExpense;
                    ClearInputFields();
                    UpdateTotalAmount();
                }
                else
                {
                    MessageBox.Show("Please enter a valid amount.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please select an expense to edit.", "Edit Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (listBoxExpenses.SelectedIndex >= 0)
            {
                expenses.RemoveAt(listBoxExpenses.SelectedIndex);
                listBoxExpenses.Items.RemoveAt(listBoxExpenses.SelectedIndex);
                ClearInputFields();
                UpdateTotalAmount();
            }
            else
            {
                MessageBox.Show("Please select an expense to delete.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveExpenses();
            MessageBox.Show("Expenses saved successfully.", "Save Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadExpenses()
        {
            if (File.Exists(FilePath))
            {
                string[] lines = File.ReadAllLines(FilePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 3 && decimal.TryParse(parts[1], out decimal amount))
                    {
                        Expense expense = new Expense(parts[0], amount, DateTime.Parse(parts[2]));
                        expenses.Add(expense);
                        listBoxExpenses.Items.Add(expense);
                    }
                }
            }
        }

        private void SaveExpenses()
        {
            List<string> lines = expenses.Select(e => $"{e.Description}|{e.Amount}|{e.Date}").ToList();
            File.WriteAllLines(FilePath, lines);
        }

        private void ClearInputFields()
        {
            textBoxDescription.Clear();
            textBoxAmount.Clear();
            dateTimePickerDate.Value = DateTime.Today;
        }

        private void UpdateTotalAmount()
        {
            decimal totalAmount = expenses.Sum(e => e.Amount);
            labelTotalAmount.Text = totalAmount.ToString("C");
        }
    }

    public class Expense
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public Expense(string description, decimal amount, DateTime date)
        {
            Description = description;
            Amount = amount;
            Date = date;
        }

        public override string ToString()
        {
            return $"{Description} - {Amount:C} - {Date:d}";
        }
    }
}
