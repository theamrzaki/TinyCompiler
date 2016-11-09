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

namespace Tiny
{

    public struct MyData
    {
        public string Tokken { set; get; }
        public string Value { set; get; }
    }

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


           special_char.Add("+", " plus ");
           special_char.Add("-", " minus ");
           special_char.Add("*", " times ");
           special_char.Add("/", " divide ");
           special_char.Add("=", " equal ");
           special_char.Add("<", " smaller ");
           special_char.Add("(", " left_bracket ");
           special_char.Add(")", " right_bracket ");
          // special_char.Add(";", " semi_col ");
           special_char.Add(":", " assignment ");

        }



        private void compile_Click(object sender, RoutedEventArgs e)
        {
            myDataGrid.Items.Clear();
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
                                            add_to_data_grid("reserved word" , "read");
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
                                                    add_to_data_grid("reserved word", "repeat");

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
                                            add_to_data_grid("reserved word", "else");

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
                                        add_to_data_grid("reserved word", "end");

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
                                    add_to_data_grid("reserved word", "if");

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
                                            add_to_data_grid("reserved word", "then");

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
                                                add_to_data_grid("reserved word", "until");

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
                                                add_to_data_grid("reserved word", "write");

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
                                add_to_data_grid("special char", "assignment");

                            }
                        }
                        else
                        {
                            //--  output.Text += item.Value;
                            add_to_data_grid("special char", item.Value);

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
                        input_text[a + 1] == '+' || input_text[a + 1] == '-' || input_text[a + 4] == '*' ||
                        input_text[a + 1] == '/' || input_text[a + 1] == '=' || input_text[a + 4] == '<' ||
                        input_text[a + 1] == '(' || input_text[a + 1] == ')' || input_text[a + 4] == ';' ||
                       (input_text[a + 1] == ':')&&(input_text[a + 2] == '='))
                    {
                        i = a;
                        //--    output.Text += "<variable: "+ variable + " >";
                        add_to_data_grid("variable", variable);

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

                            add_to_data_grid("number_float", number_variable);

                        }
                        else
                        {
                            //--   output.Text += "<number_int: " + number_variable + " >";

                            add_to_data_grid("number_int", number_variable);

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
                        add_to_data_grid("special char", "semi col");

                    }
                    //--  output.Text += "\n";

                  //  add_to_data_grid(" ", " ");

                }




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

            get_comments(input.Document);
            
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

      
        private void add_to_data_grid(string t, string v)
        {
            myDataGrid.Items.Add(new MyData { Tokken = t, Value = v });
        }

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
                   MatchCollection matches = Regex.Matches(textRun, pattern,RegexOptions.Singleline);
                   
                   foreach (Match match in matches)
                   {
                       int startIndex = match.Index;
                       int length = match.Length;
                        Colorize(startIndex, length,Colors.Green);
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

        private void input_KeyUp(object sender, KeyEventArgs e)
        {
            
            IEnumerable<TextRange> wordRanges_black = all_black(input.Document);
            foreach (TextRange wordRange in wordRanges_black)
            {
                wordRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
            }

            get_comments(input.Document);

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



            IEnumerable<TextRange> numRanges = get_numbers(input.Document);
            foreach (TextRange wordRange in numRanges)
            {
                wordRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
            }


        }
    }
}
