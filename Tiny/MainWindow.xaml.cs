using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Tiny
{

    public struct MyData
    {
        public string Tokken { set; get; }
        public string Value { set; get; }
    }

    enum shape_type
    {
        rectangel,
        circle
    };
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, string> reserved_words = new Dictionary<string, string>();
        Dictionary<string, string> special_char = new Dictionary<string, string>();

        enum tiny_type
        {
            reserved_word,
            special_char,
            variable,
            comment,
            number
        };

        

        public MainWindow()
        {
            InitializeComponent();

            DataGridTextColumn col1 = new DataGridTextColumn();
            DataGridTextColumn col2 = new DataGridTextColumn();
            myDataGrid.Columns.Add(col1);
            myDataGrid.Columns.Add(col2);
            col1.Binding = new Binding("Tokken");
            col2.Binding = new Binding("Value");
            col1.Header = "Tokken";
            col2.Header = "Value";


            reserved_words.Add("if" , " if condition ");
           reserved_words.Add("then"," if starts ");
           
           reserved_words.Add("else", " else condition ");
           reserved_words.Add("end", " section ends ");
           
           reserved_words.Add("repeat", " loop ");
           reserved_words.Add("read", " input value ");
           
           reserved_words.Add("until", " loop till ");
           reserved_words.Add("write", " output ");


           special_char.Add("+", "plus");
           special_char.Add("-", "minus");
           special_char.Add("*", "times");
           special_char.Add("/", "divide");
           special_char.Add("=", "equal");
           special_char.Add("<", "smaller");
           special_char.Add("(", "left_bracket");
           special_char.Add(")", "right_bracket");
          // special_char.Add(";"," semi_col ");
           special_char.Add(":", "assignment");

        }



        private void compile_Click(object sender, RoutedEventArgs e)
        {
            myDataGrid.Items.Clear();
            tree = "";
            index = 0;
            //string richText = new TextRange(input.Document.ContentStart, input.Document.ContentEnd).Text;
            //--------------------->> parse_comment(input.Text);

            // RichTextBox _RichTextBox = new RichTextBox(); //Initialize a new RichTextBox of name _RichTextBox
            // _RichTextBox.Select(0, 8); //Select text within 0 and 8
            //--  output.Text = "";

            //input.AppendText("1111111111111111111111111111111111111111111111111111111111111111111111");
            //input.HorizontalAlignment = HorizontalAlignment.Center;
            //input.Focus();

            string input_text = " " 
                + new TextRange(input.Document.ContentStart, input.Document.ContentEnd).Text
                + "                  ";

          //  string input_text = " "+ input+"                  ";
            for (int i = 1; i < input_text.Length; i++)
            {
                //comments
                int c = i;
                int old_c = c;
                if (input_text[c] == '{')
                {
                    while (true)
                    {
                        if (input_text[c] == '}')
                        {
                            i = c;
                            color_selection(old_c-1,c,tiny_type.comment);
                            break;
                        }
                        c++;
                    }
                }

                //reserve words
                try
                {
                    if (input_text[i - 1] == ' ' || input_text[i - 1] == '\r'
                        || input_text[i - 1] == '\n' || input_text[i - 1] == ';')
                        //you test on \n as you look for prev char 
                        //and you may have added \n
                    {
                        #region read repeat
                        if (input_text[i] == 'r') //read //repeat
                        {
                            int y = i;
                            int old_y = y;
                            if (input_text[i + 1] == 'e')
                            {
                                if (input_text[i + 2] == 'a')//read
                                {
                                    if (input_text[i + 3] == 'd')
                                    {
                                        if (input_text[i + 4] == ' ' || input_text[i + 4] == '\r' ||
                                            input_text[i + 4] == '+' || input_text[i + 4] == '-' || input_text[i + 4] == '*' ||
                                            input_text[i + 4] == '/' || input_text[i + 4] == '=' || input_text[i + 4] == '<' ||
                                            input_text[i + 4] == '(' || input_text[i + 4] == ')' || input_text[i + 4] == ';' ||
                                           (input_text[i + 4] == ':') && (input_text[i + 5] == '='))
                                        {
                                            //--   output.Text += "<read>";
                                            add_to_data_grid("reserved word" , "read" , Colors.Blue);
                                            i = y + 3;
                                            color_selection(old_y, i+2, tiny_type.reserved_word);
                                            continue;
                                        }
                                    }
                                }

                                if (input_text[i + 2] == 'p')//repeat
                                {
                                    if (input_text[i + 3] == 'e')
                                    {
                                        if (input_text[i + 4] == 'a')
                                        {
                                            if (input_text[i + 5] == 't')
                                            {
                                                if (input_text[i + 6] == ' ' || input_text[i + 6] == '\r' ||
                                                    input_text[i + 6] == '+' || input_text[i + 6] == '-' || input_text[i + 6] == '*' ||
                                                    input_text[i + 6] == '/' || input_text[i + 6] == '=' || input_text[i + 6] == '<' ||
                                                    input_text[i + 6] == '(' || input_text[i + 6] == ')' || input_text[i + 6] == ';' ||
                                                   (input_text[i + 6] == ':') && (input_text[i + 7] == '='))
                                                {
                                                    //--    output.Text += "<repeat>";
                                                    add_to_data_grid("reserved word", "repeat", Colors.Blue);

                                                    i = y + 5;
                                                    color_selection(old_y, i+3, tiny_type.reserved_word);

                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region else end
                        if (input_text[i] == 'e') //else //end
                        {
                            int y = i;
                            if (input_text[i + 1] == 'l')
                            {
                                if (input_text[i + 2] == 's')//else
                                {
                                    if (input_text[i + 3] == 'e')
                                    {
                                        if (input_text[i + 4] == ' ' || input_text[i + 4] == '\r' ||
                                            input_text[i + 4] == '+' || input_text[i + 4] == '-' || input_text[i + 4] == '*' ||
                                            input_text[i + 4] == '/' || input_text[i + 4] == '=' || input_text[i + 4] == '<' ||
                                            input_text[i + 4] == '(' || input_text[i + 4] == ')' || input_text[i + 4] == ';' ||
                                           (input_text[i + 4] == ':') && (input_text[i + 5] == '='))
                                        {
                                            //--  output.Text += "<else>";
                                            add_to_data_grid("reserved word", "else", Colors.Blue);

                                            i = y + 3;
                                            continue;
                                        }
                                    }
                                }

                            }
                            if (input_text[i + 1] == 'n')//end
                            {
                                if (input_text[i + 2] == 'd')
                                {
                                    if (input_text[i + 3] == ' ' || input_text[i + 3] == '\r' ||
                                        input_text[i + 3] == '+' || input_text[i + 3] == '-' || input_text[i + 3] == '*' ||
                                        input_text[i + 3] == '/' || input_text[i + 3] == '=' || input_text[i + 3] == '<' ||
                                        input_text[i + 3] == '(' || input_text[i + 3] == ')' || input_text[i + 3] == ';' ||
                                       (input_text[i + 3] == ':') && (input_text[i + 4] == '='))
                                    {
                                        //--     output.Text += "<end>";
                                        add_to_data_grid("reserved word", "end", Colors.Blue);

                                        i = y + 2;
                                        continue;
                                    }
                                }

                            }
                        }
                        #endregion

                        #region if
                        if (input_text[i] == 'i') //if
                        {
                            int y = i;
                            if (input_text[i + 1] == 'f')
                            {
                                if (input_text[i + 2] == ' ' || input_text[i + 2] == '\r' ||
                                    input_text[i + 2] == '+' || input_text[i + 2] == '-' || input_text[i + 2] == '*' ||
                                    input_text[i + 2] == '/' || input_text[i + 2] == '=' || input_text[i + 2] == '<' ||
                                    input_text[i + 2] == '(' || input_text[i + 2] == ')' || input_text[i + 2] == ';' ||
                                   (input_text[i + 2] == ':') && (input_text[i + 3] == '='))//else
                                {
                                    //--   output.Text += "<if>";
                                    add_to_data_grid("reserved word", "if", Colors.Blue);

                                    i = y + 1;
                                    continue;
                                }
                            }
                        }
                        #endregion

                        #region then
                        if (input_text[i] == 't') //then
                        {
                            int y = i;
                            if (input_text[i + 1] == 'h')
                            {
                                if (input_text[i + 2] == 'e')
                                {
                                    if (input_text[i + 3] == 'n')
                                    {
                                        if (input_text[i + 4] == ' ' || input_text[i + 4] == '\r' ||
                                            input_text[i + 4] == '+' || input_text[i + 4] == '-' || input_text[i + 4] == '*' ||
                                            input_text[i + 4] == '/' || input_text[i + 4] == '=' || input_text[i + 4] == '<' ||
                                            input_text[i + 4] == '(' || input_text[i + 4] == ')' || input_text[i + 4] == ';' ||
                                           (input_text[i + 4] == ':') && (input_text[i + 5] == '='))
                                        {
                                            //--      output.Text += "<then>";
                                            add_to_data_grid("reserved word", "then", Colors.Blue);

                                            i = y + 3;
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region until
                        if (input_text[i] == 'u') //until
                        {
                            int y = i;
                            if (input_text[i + 1] == 'n')
                            {
                                if (input_text[i + 2] == 't')
                                {
                                    if (input_text[i + 3] == 'i')
                                    {
                                        if (input_text[i + 4] == 'l')
                                        {
                                            if (input_text[i + 5] == ' ' || input_text[i + 5] == '\r' ||
                                                input_text[i + 5] == '+' || input_text[i + 5] == '-' || input_text[i + 5] == '*' ||
                                                input_text[i + 5] == '/' || input_text[i + 5] == '=' || input_text[i + 5] == '<' ||
                                                input_text[i + 5] == '(' || input_text[i + 5] == ')' || input_text[i + 5] == ';' ||
                                               (input_text[i + 5] == ':') && (input_text[i + 6] == '='))
                                            {
                                                //--   output.Text += "<until>";
                                                add_to_data_grid("reserved word", "until", Colors.Blue);

                                                i = y + 4;
                                                continue;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region write
                        if (input_text[i] == 'w') //write
                        {
                            int y = i;
                            if (input_text[i + 1] == 'r')
                            {
                                if (input_text[i + 2] == 'i')
                                {
                                    if (input_text[i + 3] == 't')
                                    {
                                        if (input_text[i + 4] == 'e')
                                        {
                                            if (input_text[i + 5] == ' ' || input_text[i + 5] == '\r' ||
                                                input_text[i + 5] == '+' || input_text[i + 5] == '-' || input_text[i + 5] == '*' ||
                                                input_text[i + 5] == '/' || input_text[i + 5] == '=' || input_text[i + 5] == '<' ||
                                                input_text[i + 5] == '(' || input_text[i + 5] == ')' || input_text[i + 5] == ';' ||
                                               (input_text[i + 5] == ':') && (input_text[i + 6] == '='))
                                            {
                                                //--     output.Text += "<write>";
                                                add_to_data_grid("reserved word", "write", Colors.Blue);

                                                i = y + 4;
                                                continue;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
                catch
                { }
               
                //special char
                foreach (var item in special_char)
                {
                    if (input_text[i].ToString() == item.Key.ToString()) //read //repeat
                    {
                        if(input_text[i].ToString() ==":")
                        {
                            if (input_text[i+1].ToString() == "=")
                            {
                                i = i + 1;
                                //--   output.Text += item.Value;
                                add_to_data_grid("special char", "assignment", Colors.Purple);

                            }
                        }
                        else
                        {
                            //--  output.Text += item.Value;
                            add_to_data_grid("special char", item.Value, Colors.Purple);

                        }
                    }
                }

                //variables
                //not reserved & not special char

                string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                char[] alphas = (alphabet + alphabet.ToLower()).ToCharArray();
                alphabet = alphabet + "_";
                string variable="";
                int a = i;
                while ( alphabet.Contains(input_text[a]) || alphas.Contains(input_text[a])  )
                {
                    variable += input_text[a];
                    if (input_text[a + 1] == ' ' || input_text[a + 1] == '\r' ||
                        input_text[a + 1] == '+' || input_text[a + 1] == '-' || input_text[a + 1] == '*' ||
                        input_text[a + 1] == '/' || input_text[a + 1] == '=' || input_text[a + 1] == '<' || input_text[a + 1] == '>' ||
                        input_text[a + 1] == '(' || input_text[a + 1] == ')' || input_text[a + 1] == ';' ||
                       (input_text[a + 1] == ':')&&(input_text[a + 2] == '='))
                    {
                        i = a;
                        //--    output.Text += "<variable: "+ variable + " >";
                        add_to_data_grid("variable", variable, Colors.Gray);

                        break;
                    }
                    a++;
                }

                //numbers 
                string numbers = "0123456789.";
                string number_variable = "";
                int b = i;
                while (numbers.Contains(input_text[b]))
                {
                    number_variable += input_text[b];
                    if (input_text[b + 1] == ' ' || input_text[b + 1] == '\r' ||
                        input_text[b + 1] == '+' || input_text[b + 1] == '-' || input_text[b + 1] == '*' ||
                        input_text[b + 1] == '/' || input_text[b + 1] == '=' || input_text[b + 1] == '<' ||
                        input_text[b + 1] == '(' || input_text[b + 1] == ')' || input_text[b + 1] == ';' ||
                       (input_text[b + 1] == ':')&&(input_text[b + 2] == '='))
                    {
                        i = b;
                        if(number_variable.Contains("."))
                        {
                            //--     output.Text += "<number_float: " + number_variable + " >";

                            add_to_data_grid("number_float", number_variable, Colors.Red);

                        }
                        else
                        {
                            //--   output.Text += "<number_int: " + number_variable + " >";

                            add_to_data_grid("number_int", number_variable, Colors.Red);

                        }
                        break;
                    }
                    b++;
                }
                
                //end line
                if (input_text[i] == ';' || input_text[i] == '\n') 
                {
                    if (input_text[i] == ';')
                    {
                        //--  output.Text += "<semi col>";
                        add_to_data_grid("special char", "semi col", Colors.Purple);

                    }
                    //--  output.Text += "\n";

                  //  add_to_data_grid(" ", " ");

                }

                //if (input_text[i] == '}')
                //{
                //    get_comments(input.Document);

                //}

                #region comment
                //using (StringReader sr = new StringReader(input.Text))
                //{
                //    string line;
                //    while ((line = sr.ReadLine()) != null)
                //    {
                //        string[] words = line.Split(' ');
                //        bool was_reserved = false;
                //        int i = 0;
                //        foreach (var w in words)
                //        {
                //            foreach (var d in reserved_words)
                //            {
                //                //if word is reserved
                //                if (w == d.Key)
                //                {
                //                    was_reserved = true;
                //                    output.Text += "<" + d.Value + ">";
                //                }

                //            }
                //            //see if it contains a special
                //            if(!was_reserved)
                //            {
                //                foreach (var sp_char in special_char)
                //                {
                //                    if (w == sp_char.Key)
                //                    {
                //                        was_reserved = false;
                //                        output.Text += "<" + sp_char.Key + ">";
                //                    }
                //                    else if (w.Contains(sp_char.Key))
                //                    {
                //                        was_reserved = false;
                //                        output.Text += "<long exp" + sp_char.Key + ">";
                //                    }
                //                }
                //            }
                //            was_reserved = false;

                //            i++;
                //        }


                //        output.Text += " \n";

                //        //if (output.Text.Equals(""))
                //        //{
                //        //    output.Text += line;
                //        //}
                //        //else
                //        //{
                //        //    output.Text += "$$ \n" + line;
                //        //}
                //    }
                //}
                #endregion
            }
            try
            {
                syntax_canvas.Children.Clear();

                tree += "<amr><node type = \"start\" >";
                parse();
                tree += "</node></amr>";
                Draw_Click();

                temp_factor = "";
                tree = "";
                index = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("please put a semi col... error was in "+ index);
                temp_factor = "";
                tree = "";
                index = 0;
            }
            



            IEnumerable<TextRange> wordRanges = GetAllWordRanges(input.Document);
            foreach (TextRange wordRange in wordRanges)
            {
                foreach (var item in reserved_words)
                {
                    if (wordRange.Text == item.Key)
                    {
                        wordRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
                    }
                }

                foreach (var item in special_char)
                {
                    if (wordRange.Text == item.Key)
                    {
                        wordRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
                    }
                }

                if (wordRange.Text == "{")
                {
                    wordRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.HotPink);
                }

            }


            IEnumerable<TextRange> comments_line = get_comments_line(input.Document);

            foreach (TextRange wordRange in comments_line)
            {
                wordRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
            }

            //IEnumerable<TextRange> CommentRanges = get_comments(input.Document);
            //foreach (TextRange wordRange in CommentRanges)
            //     {
            //                 wordRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
            //     }
            //
            //IEnumerable<TextRange> numRanges = get_numbers(input.Document);
            //foreach (TextRange wordRange in numRanges)
            //{
            //    wordRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
            //}

        }


        List<string> tokken_List = new List<string>();
        private void add_to_data_grid(string t, string v,Color c)
        {
            DataGridRow r = new DataGridRow();
            MyData d = new MyData { Tokken = t, Value = v };
            r.Item = d;
            r.Foreground = new SolidColorBrush(c);

            tokken_List.Add(v);
            myDataGrid.Items.Add(r);

            //myDataGrid.Items.Add(new MyData { Tokken = t, Value = v });
        }

        #region coloring 
        public static IEnumerable<TextRange> GetAllWordRanges(FlowDocument document)
        {
            string pattern = @"[^\W\d](\w|[-']{1,2}(?=\w))*";

            TextPointer pointer = document.ContentStart;
            while (pointer != null)
            {
                if (pointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string textRun = pointer.GetTextInRun(LogicalDirection.Forward);
                    MatchCollection matches = Regex.Matches(textRun, pattern);
                    foreach (Match match in matches)
                    {
                        int startIndex = match.Index;
                        int length = match.Length;
                        TextPointer start = pointer.GetPositionAtOffset(startIndex);
                        TextPointer end = start.GetPositionAtOffset(length);
                        yield return new TextRange(start, end);
                    }
                }

                pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
            }
        }

        public static IEnumerable<TextRange> get_comments_line(FlowDocument document)
        {
            string pattern = @"{.*?\}";

            TextPointer pointer = document.ContentStart;
            while (pointer != null)
            {
                if (pointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string textRun = pointer.GetTextInRun(LogicalDirection.Forward);
                    MatchCollection matches = Regex.Matches(textRun, pattern);
                    foreach (Match match in matches)
                    {
                        int startIndex = match.Index;
                        int length = match.Length;
                        TextPointer start = pointer.GetPositionAtOffset(startIndex);
                        TextPointer end = start.GetPositionAtOffset(length);
                        yield return new TextRange(start, end);
                    }
                }

                pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
            }
        }

        public static IEnumerable<TextRange> get_numbers(FlowDocument document)
        {
            //string pattern = @"{ ([A-za-z0-9]* |(\n)* )* }";
            string pattern = @"([0-9]*)";

            TextPointer pointer = document.ContentStart;
            while (pointer != null)
            {
                if (pointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string textRun = pointer.GetTextInRun(LogicalDirection.Forward);
                    MatchCollection matches = Regex.Matches(textRun, pattern);
                    foreach (Match match in matches)
                    {
                        int startIndex = match.Index;
                        int length = match.Length;
                        TextPointer start = pointer.GetPositionAtOffset(startIndex);
                        TextPointer end = start.GetPositionAtOffset(length);
                        yield return new TextRange(start, end);
                    }
                }

                pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
            }
        }

        public static IEnumerable<TextRange> all_black(FlowDocument document)
        {
            //string pattern = @"{ ([A-za-z0-9]* |(\n)* )* }";
            string pattern = @"^(.+)$";

            TextPointer pointer = document.ContentStart;
            while (pointer != null)
            {
                if (pointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string textRun = pointer.GetTextInRun(LogicalDirection.Forward);
                    MatchCollection matches = Regex.Matches(textRun, pattern);
                    foreach (Match match in matches)
                    {
                        int startIndex = match.Index;
                        int length = match.Length;
                        TextPointer start = pointer.GetPositionAtOffset(startIndex);
                        TextPointer end = start.GetPositionAtOffset(length);
                        yield return new TextRange(start, end);
                    }
                }

                pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
            }
        }
        #endregion


        #region parser
        int index=0;
        string tree = "";
        //root function ==> statement looper
        private void parse()
        {
            stmt_equence();
        }

        private void stmt_equence()
        {
            if (index == tokken_List.Count) return;
            switch (tokken_List[index])
            {
                case "if":
                    Match_if();
                    break;
                case "repeat":
                    Match_repeat();
                    break;
                case "read":
                    Match_read();
                    break;
                case "write":
                    Match_write();
                    break;
                case "semi col":
                    index++;
                    stmt_equence();
                    break;
                case "end":
                    return;
                    break;
                default:
                    Match_assign();
                    break;
            }
        }

        //function for the if condition
        private void Match_if()//tested
        {
            tree += "<node type=\"if\" >";
            index++;
            tree += "<node type=\"condition\" >";
            Match_expression();
            tree += "</node>";

            //now we expect then
            if (tokken_List[index] == "then")
            {
                tree += "<node type=\"then\" >";
                index++;
                stmt_equence();
                tree += "</node>";
                //tree += "</node>";

                //now we excpect else or end
                if (tokken_List[index-1] == "else" || tokken_List[index] == "end")
                {
                    if (tokken_List[index-1]== "else")
                    {
                        tree += "<node type=\"else\" >";
                        stmt_equence();
                        tree += "</node>";
                        if (tokken_List[index] == "end")
                        {
                            index++;
                            tree += "</node>";
                            stmt_equence();
                        }
                    }
                    else if (tokken_List[index] == "end")
                    {
                        index++;
                        tree += "</node>";
                        stmt_equence();
                    }
                }
                else
                {
                    MessageBox.Show("error at " + index);
                }
            }
            else
            {
                MessageBox.Show("error at " + index);
            }
        }
        
        private void Match_repeat()//tested
        {
            tree += "<node type=\"repeat\" >";

            tree += "<node type=\"loop_part\" >";
            index++;
            stmt_equence();
            tree += "</node>";
            
            //now we expect until
            if (tokken_List[index-1] == "until")
            {
                tree += "<node type=\"condition_part\" >";
                Match_expression();
                tree += "</node>";

                index++;
                stmt_equence();

                //     tree += "</node>";
            }
            else
            {
                //MessageBox.Show("error at " + index);
            }
        }
        
        private void Match_read()//tested
        {
            index = index + 1;
            tree += "<node type=\"read (" + tokken_List[index] + ") \" />";
            index = index + 1;

            stmt_equence();
        }

        private void Match_write()//tested
        {
            tree += "<node type=\"write\" >";
            index++;
            Match_expression();

            index++;
            stmt_equence();

        }

        private void Match_assign()//tested
        {
            index ++; //id
            if (tokken_List[index] == "assignment")
            {
                tree += "<node type=\"assign (" + tokken_List[index-1] + ") \">";
                index++;

                if (index == tokken_List.Count()) return;

                Match_expression();
                //index = index + 2;//must be 1

                if (tokken_List[index] == "semi col")
                {
                    index++;
                    stmt_equence();
                }
            }
            else
            {
                if (tokken_List[index] == "semi col")
                {
                    index++;
                    stmt_equence();
                }
                //return;
                //MessageBox.Show("error at " + index);
            }
        }



        //-----------------------------------expression----------------------------------------------------------
        string temp_factor="";
        bool was_exp = false;
        private void Match_expression()
        {
            Match_term();
            index++;

            if (tokken_List[index] == "plus" || tokken_List[index] == "minus" || tokken_List[index] == "equal" ||
                    tokken_List[index] == "smaller" ||
                    tokken_List[index] == "bigger")
            {
                was_exp = true;
                tree += "<node type=\""+ tokken_List[index] + "\" shape=\"circle\" >" + temp_factor;
                index++;
                Match_term();
            }
            #region commented
            //else if(tokken_List[index] == "equal" ||
            //        tokken_List[index] == "smaller" ||
            //        tokken_List[index] == "bigger")
            //{
            //    was_exp = true;

            //    tree += "<node type=\"" + tokken_List[index] + "\" shape=\"circle\" />";
            //        index++;
            //        Match_expression();
            //        //tree += "</node>";
            //}
            #endregion
            if (tokken_List[index] != "semi col")
            {
                if (tokken_List[index] != "then")
                {
                    was_exp = false;
                    Match_expression();
                    tree += "</node>"; //of plus | minus | ...
                }
                else
                {
                    tree += temp_factor;
                }
            }
            else if (tokken_List[index] == "semi col"  || tokken_List[index] == "then")
            {
                tree += temp_factor;
                tree += "</node>";//of assign
                //index++;
               // stmt_equence();
            }
        }
        private void Match_term()
        {
            Match_factor();
            if (tokken_List[index] == "times")
            {
                tree += "<node type=\"" + tokken_List[index] + "\" shape=\"circle\" />";
                index++;
                Match_factor();
                tree += "</node>";
            }
            else
            {
            }
        }
        private void Match_factor()
        {
            if (tokken_List[index] == "left_bracket")
            {
                index++;
                Match_expression();
                if (tokken_List[index] == "right_bracket")
                {
                    index++;
                }
                else
                {
                    MessageBox.Show("no right bracket at tokken" + index);
                }
            }
            else
            {
                temp_factor = "<node type=\"" + tokken_List[index] + "\" shape=\"circle\" />";
                //index++;
            }

        }

        #endregion

        #region unused

        #region comments
        public void get_comments(FlowDocument document)
        {
            //string pattern = @"{ ([A-za-z0-9]* |(\n)* )* }";
            // string pattern = @"{[A-za-z0-9-$]*}";
            // string pattern = @"/\*.*?\*/";

            string pattern = @"{.*?\}";

            // TextPointer pointer = document.ContentStart;

            // string a = new TextRange(input.Document.ContentStart, input.Document.ContentEnd).Text + "   ";

            TextPointer pointer = input.Document.ContentStart.GetInsertionPosition(LogicalDirection.Forward);
            // Run r = new Run(a, tp);

            //while (pointer != null)
            //{
            //     if (pointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
            //     {
            string textRun = new TextRange(input.Document.ContentStart, input.Document.ContentEnd).Text;
            //  string textRun = pointer.GetTextInRun(LogicalDirection.Forward);
            MatchCollection matches = Regex.Matches(textRun, pattern, RegexOptions.Singleline);

            foreach (Match match in matches)
            {
                int startIndex = match.Index;
                int length = match.Length;
                Colorize(startIndex, length, Colors.Green);
                // TextPointer start = pointer.GetPositionAtOffset(startIndex);
                // TextPointer end = pointer.GetPositionAtOffset(length);

                input.Selection.Select(input.Document.ContentEnd, input.Document.ContentEnd);
                //yield return new TextRange(start, end);
            }
            //     }

            //    pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
            //}
        }

        private static TextPointer GetPoint(TextPointer start, int x)
        {
            var ret = start;
            var i = 0;
            while (i < x && ret != null)
            {
                if (ret.GetPointerContext(LogicalDirection.Backward) ==
        TextPointerContext.Text ||
                    ret.GetPointerContext(LogicalDirection.Backward) ==
        TextPointerContext.None)
                    i++;
                if (ret.GetPositionAtOffset(1,
        LogicalDirection.Forward) == null)
                    return ret;
                ret = ret.GetPositionAtOffset(1,
        LogicalDirection.Forward);
            }
            return ret;
        }

        private void Colorize(int offset, int length, Color color)
        {
            var textRange = input.Selection;
            var start = input.Document.ContentStart;
            var startPos = GetPoint(start, offset);
            var endPos = GetPoint(start, offset + length);

            textRange.Select(startPos, endPos);
            textRange.ApplyPropertyValue(TextElement.ForegroundProperty,
            new SolidColorBrush(color));

            //startPos = GetPoint(start, 0);
            //endPos = GetPoint(start, 0 );
            //
            //textRange.Select(startPos, endPos);

        }
        #endregion

        private void color_selection(int start, int end, tiny_type t)
        {
            //TextPointer pointer = input.Document.ContentStart;
            //TextPointer starta = pointer.GetPositionAtOffset(start);

            //string input_text = new TextRange(input.Document.ContentStart, input.Document.ContentEnd).Text;
            // end= input_text.IndexOf("}", start);
            //TextPointer enda = starta.GetPositionAtOffset(end);
            //TextRange wordRange = new TextRange(starta, enda);
            //wordRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.HotPink);


            //input.UpdateLayout();
            //input.ScrollToEnd();
            //input.UpdateLayout();

            //TextPointer text = input.Document.ContentStart;
            ////while (text.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
            //// {
            ////     text = text.GetNextContextPosition(LogicalDirection.Forward);
            //// }

            //TextPointer start_pos = input.Document.ContentStart.GetPositionAtOffset(start);
            //TextPointer end_pos = input.Document.ContentStart.GetPositionAtOffset(end);
            //TextRange rangeOfText2 = new TextRange(start_pos,
            // end_pos);
            // string k= rangeOfText2.Text;

            //input.Focus();
            //input.Selection.Select(start_pos, end_pos);
            //// rangeOfText2.Text = "RED !";
            //input.BringIntoView();
            //rangeOfText2.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);

            //   output.Focus();


            //input.Focus();
            //
            //input.Selection.Select(input.Document.ContentEnd, input.Document.ContentEnd);
            //TextSelection tsA = input.Selection;
            //tsA.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);

            // TextPointer caretBack = input.CaretPosition.GetPositionAtOffset(-start);
            // TextPointer endPosq = input.CaretPosition.GetPositionAtOffset(-end);

            // TextRange rtbText = new TextRange(caretBack, endPosq);
            // string ttt = rtbText.Text;
            // input.Selection.Select(caretBack, endPosq);

            // // input.Focus();
            // //
            // // TextPointer text = input.Document.ContentStart;
            // //
            // // while (text.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
            // // {
            // //     text = text.GetNextContextPosition(LogicalDirection.Forward);
            // // }
            // // TextPointer startPos = text.GetPositionAtOffset(start);
            // // TextPointer endPos = text.GetPositionAtOffset(end);



            // // input.Selection.Select(startPos, endPos);
            //// this.input.SelectionBrush = System.Windows.Media.Brushes.Green;

            // TextSelection ts = input.Selection;
            // if (ts != null)
            // {
            //     if (t == tiny_type.comment)
            //     {
            //         ts.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
            //     }
            //     else if (t == tiny_type.reserved_word)
            //     {
            //         ts.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
            //     }
            //     else if (t == tiny_type.special_char)
            //     {
            //         ts.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);

            //         this.input.SelectionBrush = System.Windows.Media.Brushes.Black;
            //     }
            //     else if (t == tiny_type.variable)
            //     {
            //         ts.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Cyan);
            //     }
            //     else if (t == tiny_type.number)
            //     {
            //         ts.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
            //     }
            // }

            // // startPos = text.GetPositionAtOffset(end);
            // // endPos = text.GetPositionAtOffset(end);
            // // input.Selection.Select(startPos, endPos);
            // // TextSelection tsA = input.Selection;
            // // tsA.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
            // // input.Focus();

        }

        //private void parse_comment(string text)
        //{
        //    int comment_start =0;
        //    int comment_end = -1;
        //    int total_start=0, total_end=0;
        //    string sub = text;

        //    try
        //    {
        //        while (sub.IndexOf('{') != -1)
        //        {
        //            comment_start = sub.IndexOf('{');
        //            comment_end = sub.IndexOf('}');

        //            total_start += comment_start;
        //            total_end += total_start + comment_end;

        //            //--    output.Text += "comment started at " + total_start + " and ended at " + total_end + " \n";
        //            sub = text.Substring(comment_end + 1);

        //        }
        //    }
        //    catch
        //    {

        //    }


        //}
        #endregion


        private void input_KeyUp(object sender, KeyEventArgs e)
        {
            
            IEnumerable<TextRange> wordRanges_black = all_black(input.Document);
            foreach (TextRange wordRange in wordRanges_black)
            {
                wordRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
            }


          

            ///----      get_comments(input.Document);

            // IEnumerable<TextRange> CommentRanges = get_comments(input.Document);
            // foreach (TextRange wordRange in CommentRanges)
            // {
            //     wordRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
            // }

            IEnumerable<TextRange> wordRanges = GetAllWordRanges(input.Document);
            foreach (TextRange wordRange in wordRanges)
            {
                foreach (var item in reserved_words)
                {
                    if (wordRange.Text == item.Key)
                    {
                        wordRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
                    }
                }
            }
            
            IEnumerable<TextRange> numRanges = get_numbers(input.Document);
            foreach (TextRange wordRange in numRanges)
            {
                wordRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
            }



            IEnumerable<TextRange> wordRanges_commet = get_comments_line(input.Document);
            foreach (TextRange wordRange in wordRanges_commet)
            {
                wordRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
            }
        }




        #region drawer
        int space = 20;
        private void Draw_Click()
        {
            #region xaml example
            //            string file = @"<amr>
            //                <node type=""0"">
            //                <node type=""if0"" ><node type=""if1"">
            //                                <node type=""if2""><node type=""if3""/><node type=""if4""/></node>
            //                                <node type=""if5""><node type=""if6""/></node>
            //                            </node>
            //                </node>

            //                <node type=""while0"" ><node type=""while1"">
            //                                <node type=""while2""/>
            //                                <node type=""while3""/>
            //                                <node type=""while4""/>
            //                                <node type=""while5""><node type=""while6""><node type=""while7""/></node></node>
            //                            </node>
            //                </node>
            //                </node>
            //</amr>";

            //string file = @"<amr>
            //<node type=""start"">
            //                <node type=""if0"" >
            //                            <node type=""then part"">
            //                                <node type=""if2"">
            //                                          <node type=""2 then"">
            //                                                <node type=""inner""/>
            //                                            </node>
            //                                          <node type=""2 else""/>  
            //                                    </node>
            //                                <node type=""if3""/>
            //                                <node type=""if4""/>
            //                                <node type=""if5""><node type=""if6""/></node>
            //                            </node>
            //                             <node type=""else part"">
            //                                <node type=""if7""/>
            //                                <node type=""if8""/>
            //                                <node type=""if9""/>
            //                                <node type=""if10""><node type=""if11""/></node>
            //                            </node>
            //                </node>
            //</node>
            //</amr>";
            #endregion


            string file = tree;
            var doc = XDocument.Load(new StringReader(file));

            // var gridElement = doc.Root.Elements("node").Where(p => p.Attribute("type").Value == "if");
            var gridElement = doc.Root.Elements("node");

            Point next_point = new Point(0, 0);
            int i = 0;
            int level = 1;

            //foreach (var item in gridElement)
            //{
            //    CreateTree(item, next_point, level);
            //}
             CreateTree(gridElement.ElementAt(0), next_point, level,null);

            int k = 5;

            #region commented 3
            //foreach (var item in gridElement)
            //{
            //    CreateTree(item,next_point,level);


            //   ////// List<node> nodes = new List<node>();
            //   //// string type_of_attr = item.Attribute("type").Value;

            //   //// node n;
            //   //// if (i == 0)
            //   ////     n = new node(next_point.X, 0, type_of_attr, null);
            //   //// else
            //   ////     n = new node(next_point.X, 0, type_of_attr, nodes[i-1]);

            //   //// syntax_canvas.Children.Add(n.rect);
            //   //// syntax_canvas.Children.Add(n.text);

            //   //// next_point = new Point(n.right_point.X + space, n.right_point.Y);
            //   //// nodes.Add(n);

            //   //// //inner nodes
            //   //// if (type_of_attr == "if" || type_of_attr == "while")
            //   //// {
            //   ////     IEnumerable<XElement> inner_elements = item.Elements();
            //   ////     foreach (var inner_element in inner_elements)
            //   ////     {
            //   ////         XElement temp = inner_element;

            //   ////         node n_inner = new node(next_point.X, level * 20, inner_element.Attribute("type").Value, nodes.Last());
            //   ////         syntax_canvas.Children.Add(n_inner.rect);
            //   ////         syntax_canvas.Children.Add(n_inner.text);

            //   ////         next_point = new Point(n_inner.right_point.X + space, n_inner.right_point.Y);
            //   ////         nodes.Add(n_inner);

            //   ////         level++;

            //   ////         while (temp.HasElements)
            //   ////         {
            //   ////             foreach (var a in temp.Elements())
            //   ////             {
            //   ////                 node n_inner_inner = new node(next_point.X, level * 20, 
            //   ////                                             inner_element.Attribute("type").Value, nodes.Last());

            //   ////                 syntax_canvas.Children.Add(n_inner_inner.rect);
            //   ////                 syntax_canvas.Children.Add(n_inner_inner.text);

            //   ////                 next_point = new Point(n_inner_inner.right_point.X + space, n_inner_inner.right_point.Y);
            //   ////                 nodes.Add(n_inner_inner);
            //   ////             }
            //   ////             temp = temp.Elements().First();
            //   ////             level++;
            //   ////         }
            //   ////     }                    
            //   //// }
            //   //// i++;
            //}
            #endregion

            #region commented2
            ////draw rectangles ==>level 1
            //for (int i = 0; i < 5; i++)
            //{
            //    node n = new node(next_point.X, 0,"aaa");
            //    syntax_canvas.Children.Add(n.rect);
            //    syntax_canvas.Children.Add(n.text);

            //    next_point = new Point(n.right_point.X + space, n.right_point.Y);
            //    nodes.Add(n);

            //}
            //next_point = new Point(0, 0);

            ////draw rectangles ==> level 2
            //for (int i = 0; i < 5; i++)
            //{
            //    node n = new node(next_point.X, space*3,"bbb");
            //    syntax_canvas.Children.Add(n.rect);
            //    syntax_canvas.Children.Add(n.text);

            //    next_point = new Point(n.right_point.X + space, n.right_point.Y);
            //    nodes.Add(n);

            //}
            #endregion

            //draw inner links
            foreach (var item in nodes)
            {
                if(item.prev_node !=null)
                {

                    link l = new link(item.prev_node, item);
                    syntax_canvas.Children.Add(l.line);
                }
            }




            #region commented
            // Line line = new Line();
            // line.StrokeThickness = 1;
            // line.Stroke = Brushes.Black;
            // line.X1 = n.right_point.X;           line.Y1 = n.right_point.Y;
            // line.X2 = n.right_point.X + space;   line.Y2 = n.right_point.Y;
            // syntax_canvas.Children.Add(line);



            //Line bottom_line = new Line();
            //bottom_line.StrokeThickness = 1;
            //bottom_line.Stroke = Brushes.Black;

            //bottom_line.X1 = n.bottom_point.X;           bottom_line.Y1 = n.bottom_point.Y;
            //bottom_line.X2 = n.bottom_point.X-space;           bottom_line.Y2 = n.right_point.Y+space;

            //syntax_canvas.Children.Add(bottom_line);

            // Line bottom_line_2 = new Line();
            // bottom_line_2.StrokeThickness = 1;
            // bottom_line_2.Stroke = Brushes.Black;

            // bottom_line_2.X1 = n.bottom_point.X;         bottom_line_2.Y1 = n.bottom_point.Y;
            // bottom_line_2.X2 = n.bottom_point.X + space; bottom_line_2.Y2 = n.right_point.Y + space;

            // syntax_canvas.Children.Add(bottom_line_2);
            #endregion
        }
        HashSet<node> nodes = new HashSet<node>();
        HashSet<string> nodes_text = new HashSet<string>();
        XElement temp = new XElement("a");
        node reserved_node = new node();
        return_type CreateTree(XElement item , Point next_point , int level, node prev_node)
        {
            shape_type sh = shape_type.rectangel;
            try
            {
                if(item.Attribute("shape").Value=="circle")
                {
                    sh = shape_type.circle;
                }
            }
            catch (Exception)
            {
            }

            node node = new node(next_point.X, level, item.Attribute("type").Value, prev_node,sh);
            if(item.Attribute("type").Value != "start")
            {
                if (sh == shape_type.rectangel)
                {
                    syntax_canvas.Children.Add(node.rect);
                }
                else if(sh == shape_type.circle)
                {
                    syntax_canvas.Children.Add(node.circle);
                }
                syntax_canvas.Children.Add(node.text);

                nodes.Add(node);
                nodes_text.Add(item.Attribute("type").Value);
                level = level + 70;
            }

            temp = item;

            while (temp.HasElements) //inner elements
            {
                node pp_no = new node();
                int k = 0;
                foreach (var a in temp.Elements()) //same level
                {
                    //shape_type sssh = shape_type.rectangel;
                    node n_inner_inner = new node(next_point.X, level ,
                                                a.Attribute("type").Value, node, sh);
                    
                    next_point = new Point(n_inner_inner.right_point.X + space, n_inner_inner.right_point.Y);
                    temp = a;

                    return_type rr = new return_type();

                    //try
                    //{
                    //    if (nodes[k - 1].text_string == "then part" ||
                    // nodes[k - 1].text_string == "else part")
                    //    {
                    //        rr = CreateTree(a, next_point, level, node);
                    //    }
                    //    else
                    //    {
                    //        rr = CreateTree(a, next_point, level, n_inner_inner);
                    //    }
                    //}
                    //catch (Exception)
                    //{

                    if(sh == shape_type.circle)
                    rr = CreateTree(a, next_point, level, node);
                     
                     //}
                    if(k==0)
                    {
                        if(sh != shape_type.circle)
                        rr = CreateTree(a, next_point, level, node);

                        // rr = CreateTree(a, next_point, level, n_inner_inner);
                    }
                    else
                        if (sh != shape_type.circle)
                        rr = CreateTree(a, next_point, level, pp_no);


                    next_point = rr.return_point;
                    pp_no = rr.return_node;
                    // next_point = new Point(next_point.X + space, next_point.Y);
                    k++;
                }

                //  temp = temp.Elements().First();
            }
            return_type r = new return_type();
            r.return_node = node;
            r.return_point= next_point;

            return r;
            //while()
            //{

            //}
            //node.childCount = board.GeAvailableMoveCount();
            //node.value = board.GetValue;
            //if (depth > 0 && node.childCount > 0)
            //{
            //    node.children = new Node*[node.childCount];
            //    for (int i = 0; i != node.childCount; ++i)
            //        node.children[i] = CreateTree(board.Move(i), depth - 1);
            //}
            //else
            //{
            //    node.children = NULL;
            //}
        }
        #endregion

        private void move_right_Click(object sender, RoutedEventArgs e)
        {
            Thickness t = syntax_canvas.Margin;
            t.Left -= 100;
            syntax_canvas.Margin = t;
        }

        private void move_left_Click(object sender, RoutedEventArgs e)
        {
            Thickness t = syntax_canvas.Margin;
            t.Left += 100;
            syntax_canvas.Margin = t;
        }



        private void move_top_Click(object sender, RoutedEventArgs e)
        {
            Thickness t = syntax_canvas.Margin;
            t.Top += 100;
            syntax_canvas.Margin = t;
        }

        private void move_down_Click(object sender, RoutedEventArgs e)
        {
            Thickness t = syntax_canvas.Margin;
            t.Top -= 100;
            syntax_canvas.Margin = t;
        }





        //public Point prevScrollPoint;
        //public double pointX, pointY;
        //public double offsetX, offsetY;

        //private void syntax_canvas_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        prevScrollPoint = e.GetPosition(syntax_canvas);
        //    }

        //}

        //private void syntax_canvas_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        //new code
        //        offsetX = e.GetPosition(syntax_canvas).X - prevScrollPoint.X;
        //        offsetY = e.GetPosition(syntax_canvas).Y - prevScrollPoint.Y;

        //        pointX = -(syntax_canvas.Margin.Left);
        //        pointY = -(syntax_canvas.Margin.Top);

        //        pointX += -pointX;
        //        pointY += -pointY;


        //        Thickness t = syntax_canvas.Margin;
        //        t.Left += offsetX;
        //        t.Right += offsetY;
        //        syntax_canvas.Margin = t;

        //        //this.AutoScrollPosition = new Point(pointX, pointY);

        //        prevScrollPoint = e.GetPosition(syntax_canvas);
                
        //    }
        //}
    }

    class node
    {
       public node prev_node =null;
       public List<node> childeren = null;
        public bool visted = false;

        public Rectangle rect = new Rectangle();
        public Ellipse circle = new Ellipse();

        public TextBlock text = new TextBlock();
        public string text_string = "";

        public Point right_point = new Point();
       public Point Left_point = new Point();
       public Point bottom_point = new Point();
       public Point top_point = new Point();

       public node()
       {
       }

       public node(double left, double top,string t , node prev_nodee ,shape_type sh)
       {
            this.prev_node = prev_nodee;
            if(sh ==shape_type.rectangel)
            {
              rect.Stroke = new SolidColorBrush(Colors.Black);
              rect.Fill = new SolidColorBrush(Colors.White);
              rect.Width = 98;
              rect.Height = 50;

              Canvas.SetLeft(rect, left);
              Canvas.SetTop(rect, top);
              Canvas.SetZIndex(rect, 50);
            }
            else
            {
                circle.Stroke = new SolidColorBrush(Colors.Black);
                circle.Fill = new SolidColorBrush(Colors.White);
                circle.Width = 98;
                circle.Height = 50;

                Canvas.SetLeft(circle, left);
                Canvas.SetTop(circle, top);
                Canvas.SetZIndex(circle, 50);
            }
            visted = true;

           

            right_point = new Point(98 + left, (50 / 2) + top);
            bottom_point = new Point(((98 + left)-(98/2) ), 50 + top);
            top_point = new Point(((98 + left) - (98 / 2)), top);
            Left_point = new Point(left , (50 / 2) + top);

            text.Text = t;
            text_string = t;
            Canvas.SetLeft(text, (98 + left) - (98 / 2)-10);
            Canvas.SetTop(text, (50 / 2) + top-10);
            Canvas.SetZIndex(text, 60);

        }
    }
    class return_type
    {
        public node return_node;
        public Point return_point;
    }
    class link
    {
        public Line line = new Line();
        public link(node n1 ,node n2)
        {
            line.StrokeThickness = 1;
            line.Stroke = Brushes.Black;
            
            line.X1 = n1.right_point.X;     line.Y1 = n1.right_point.Y;
            line.X2 = n2.Left_point.X;      line.Y2 = n2.Left_point.Y;
        }
    }



}
