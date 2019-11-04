using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;

namespace calculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void btnInput_Click(object sender, EventArgs e)
        {
            Button currentButton = sender as Button;
            if (currentButton != null && currentButton.Tag != null && textBox1.Text.Length<14)
            {
                string input = currentButton.Tag.ToString();
                textBox1.Text = textBox1.Text + input;
                textBox2.Font = new System.Drawing.Font("宋体", 14.25F);
                textBox2.ForeColor = System.Drawing.Color.Silver;
                try
                {
                    string result = Identify('0'+textBox1.Text);
                    result = getResult(result);
                    textBox2.Text = result;
                }
                catch //(Exception ex)
                {
                    
                }
                
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //退格键
            string express = textBox1.Text ?? string.Empty;
            if (!string.IsNullOrEmpty(express))
            {
                textBox1.Text = express.Substring(0, express.Length - 1);
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            //清空键
            textBox1.Text = textBox2.Text = string.Empty;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            //等于键
            textBox2.Font = new System.Drawing.Font("宋体", 21.75F);
            textBox2.ForeColor = System.Drawing.SystemColors.ControlText;
            try
            {
                string result = Identify('0'+textBox1.Text);
                result = getResult(result);
                textBox2.Text = result;
            }
            catch //(Exception ex)
            {
                textBox2.Text = "错误";
                //MessageBox.Show("输入不合法,请重新输入!");
                //textBox1.Text = textBox2.Text = string.Empty;
            }
        }
        private int Priority(char c)
        {
            //判断三个优先级
            if (c == '+' || c == '-')
                return 0;
            else if (c == '*' || c == '/' || c == '%')
                return 1;
            else if (c == '^'|| c=='!'|| c== '√')
                return 2;
            else
                return 3;
        }

        private string Identify(string strExpression)
        {
            //把一般的中缀表达式变成后缀表达式
            Stack stack = new Stack();
            StringBuilder st = new StringBuilder();
            char c = ' ';

            StringBuilder sb = new StringBuilder(strExpression);
            for (int i = 0; i < sb.Length; i++)
            {
                if (char.IsDigit(sb[i]) || sb[i] == '.')
                {
                    //如果读入操作数，则直接放入输出字符串。
                    st.Append(sb[i]);
                }
                else if (sb[i] == '+' || sb[i] == '-' || sb[i] == '*' || sb[i] == '/' || sb[i] == '%' || sb[i] == '^'||sb[i]=='!'|| sb[i] == '√')
                {
                    //如果读入一般运算符如 + -*/，则放入堆栈
                    while (stack.Count > 0)
                    {
                        //放入堆栈之前必须要检查栈顶
                        c = (char)stack.Pop();
                        if (c == '(')
                        {
                            //如果读入(，因为左括号优先级最高，因此放入栈中，但是注意，当左括号放入栈中后，则优先级最低。
                            stack.Push(c);
                            break;
                        }
                        else
                        {
                            if (Priority(c) < Priority(sb[i]))
                            {
                                //确定栈顶运算符的优先级比放入的运算符的优先级低
                                stack.Push(c);
                                break;
                            }
                            else
                            {
                                //如果放入的优先级较低，则需要将栈顶的运算符放入输出字符串。
                                st.Append(' ');
                                st.Append(c);
                            }
                        }
                    }
                    stack.Push(sb[i]);
                    st.Append(' ');
                }
                else if (sb[i] == '(')
                {
                    //如果读入(，因为左括号优先级最高，因此放入栈中(嵌套括号)
                    stack.Push('(');
                }
                else if (sb[i] == ')')
                {
                    //如果读入），则将栈中运算符取出放入输出字符串
                    while (stack.Count > 0)
                    {
                        c = (char)stack.Pop();
                        if (c != '(')
                        {
                            //直到取出（为止
                            st.Append(' ');
                            st.Append(c);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    st.Append(' ');
                    st.Append(sb[i]);
                }
            }
            while (stack.Count > 0)
            {
                //顺序读完表达式，如果栈中还有操作符，则弹出，并放入输出字符串。
                st.Append(' ');
                st.Append(stack.Pop());
            }
            return st.ToString();
        }

        private string getResult(string strExpression)
        {
            //计算后缀表达式
            Stack stack = new Stack();
            string strResult = "";
            double a1, a2, result;
            int a3;
            string[] str = strExpression.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < str.Length; i++)
            {
                switch (str[i])
                {
                    //如果是操作符，则取出栈中两个操作数，进行运算后，将结果放入栈中。
                    case "*":
                        a1 = Double.Parse(stack.Pop().ToString());
                        a2 = Double.Parse(stack.Pop().ToString());
                        result = a2 * a1;
                        stack.Push(result.ToString());
                        break;
                    case "/":
                        a1 = Double.Parse(stack.Pop().ToString());
                        a2 = Double.Parse(stack.Pop().ToString());
                        result = a2 / a1;
                        stack.Push(result.ToString());
                        break;
                    case "%":
                        a1 = Double.Parse(stack.Pop().ToString());
                        result = a1 / 100;
                        stack.Push(result.ToString());
                        break;
                    case "^":
                        a1 = Double.Parse(stack.Pop().ToString());
                        a2 = Double.Parse(stack.Pop().ToString());
                        result = Math.Pow(a2, a1);
                        stack.Push(result.ToString());
                        break;
                    case "+":
                        a1 = Double.Parse(stack.Pop().ToString());
                        a2 = Double.Parse(stack.Pop().ToString());
                        result = a2 + a1;
                        stack.Push(result);
                        break;
                    case "-":
                        a1 = Double.Parse(stack.Pop().ToString());
                        a2 = Double.Parse(stack.Pop().ToString());
                        result = a2 - a1;
                        stack.Push(result);
                        break;
                    case "!":
                        a3 = int.Parse(stack.Pop().ToString());                      
                        result = a3;
                        if (result == 0) result = 1;
                        else for(int j = a3;j>1;j--)
                        {
                            result = result*(j-1);
                        }
                        str[i] = result.ToString();
                        stack.Push(Double.Parse(str[i]));
                        break;
                    case "√":
                        a1 = Double.Parse(stack.Pop().ToString());
                        result = Math.Sqrt(a1);
                        str[i] = result.ToString();
                        stack.Push(Double.Parse(str[i]));
                        break;
                    default:
                        //如果是操作数，则放入栈中。
                        stack.Push(Double.Parse(str[i]));
                        break;
                }
            }
            //直到最后栈中只有一个元素，此元素就是计算结果。
            strResult = stack.Pop().ToString();
            return strResult;
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
