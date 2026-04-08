using System;
using System.Windows.Forms;
using CalculatorWinForm.CalculatorService; // namespace của service reference

namespace CalculatorWinForm
{
    public partial class Form1 : Form
    {
        private CalculatorWslmplClient serviceClient;

        public Form1()
        {
            InitializeComponent();
            serviceClient = new CalculatorWslmplClient(); // tạo proxy client
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CallService("add");
        }

        private void btnSubtract_Click(object sender, EventArgs e)
        {
            CallService("subtract");
        }

        private void btnMultiply_Click(object sender, EventArgs e)
        {
            CallService("multiply");
        }

        private void btnDivide_Click(object sender, EventArgs e)
        {
            CallService("divide");
        }

        private void CallService(string operation)
        {
            try
            {
                int num1 = int.Parse(txtNum1.Text);
                int num2 = int.Parse(txtNum2.Text);
                double result = 0;

                switch (operation)
                {
                    case "add":
                        result = serviceClient.add(num1, num2);
                        break;
                    case "subtract":
                        result = serviceClient.subtract(num1, num2);
                        break;
                    case "multiply":
                        result = serviceClient.multiply(num1, num2);
                        break;
                    case "divide":
                        result = serviceClient.divide(num1, num2);
                        break;
                }

                lblResult.Text = "Result: " + result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}