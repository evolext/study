using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fractional_Calc
{
    public partial class MainForm : Form
    {
        // Объект класса управление
        TCtrl _Control;

        // Объект буфер обмена
        string _ClipBoard;

        Button[] controlButtons;
        Keys[] validKeys;

        public MainForm()
        {
            InitializeComponent();
            // Инициализиируем объекты
            _Control = new TCtrl();
            labelScreen.Text = _Control.DoCalculatorCommand(NumCalcCmd.ReadEditor);
            labelOperation.Text = "";
            labelMemory.Text = "";
            // Блокировка кнопок памяти
            buttonMemoryClear.Enabled = false;
            buttonMemoryRead.Enabled = false;
            _ClipBoard = System.String.Empty;
            // Запрет на изменение размера окна формы
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;

            controlButtons = new Button[] {
                                            buttonNumZero, buttonNumOne, buttonNumTwo, buttonNumThree, buttonNumFour,
                                            buttonNumFive, buttonNumSix, buttonNumSeven, buttonNumEight, buttonNumNine, 
                                            buttonBackspace, buttonOpAdd, buttonOpSub, buttonOpDiv, buttonOpMul, buttonFuncRev,
                                            buttonMemoryClear, buttonMemoryRead, buttonMemoryAdd, buttonMemorySave,
                                            buttonClearEditor, buttonFuncSqr, buttonEvalExpression, buttonSep, buttonSign
                                            
            };

            validKeys = new Keys[] { 
                                            Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9,
                                            Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9,
                                            Keys.Multiply, Keys.Add, Keys.Subtract, Keys.Divide,
                                            Keys.Oem2, Keys.Oem5, Keys.OemMinus, Keys.Back
            };

            buttonEvalExpression.Focus();

            // Настройка контекстного меню
            // Создание элементов контекстного меню
            var copyContextMenuItem = new ToolStripMenuItem("Копировать");
            var pasteContextMenuItem = new ToolStripMenuItem("Вставить");
            var cutContextMenuItem = new ToolStripMenuItem("Вырезать");
            // Добавление элементов в контексное меню
            contextMenuScreen.Items.AddRange(new[] { copyContextMenuItem, pasteContextMenuItem, cutContextMenuItem });
            // Ассоциируем контекстное меню с текстовым полем
            labelScreen.ContextMenuStrip = contextMenuScreen;
            // Добавление обработчиков на каждый элемент контекстного меню
            copyContextMenuItem.Click += copyContextMenuItem_Click;
            pasteContextMenuItem.Click += pasteContextMenuItem_Click;
            cutContextMenuItem.Click += cutContextMenuItem_Click;
        }

        // Добавление всплывающих подсказок к каждой кнопке
        private void MainForm_Load(object sender, EventArgs e)
        {
            toolTipButton = new ToolTip();
            // Тексты пподсказок
            string[] toolTipMessages = new string[] {
                "Цифра ноль", "Цифра один", "Цифра два", "Цифра три", "Цифра четыре" , "Цифра пять" , "Цифра шесть", "Цифра семь", "Цифра восемь",
                "Цифра девять", "Убрать последний символ", "Операция сложить", "Операция вычесть", "Операция разделить", "Операция умножить", 
                "Вычислить обратное значение", "Очистить память", "Прочитать из памяти", "Прибавить к значению в памяти", "Загрузить в память",
                "Очистить поле редактора", "Возвести значение в квадрат", "Получить результат", "Добавить разделитель целой и дробной части",
                "Сменить знак числа"
            };

            for (int i = 0; i < controlButtons.Length; i++)
                toolTipButton.SetToolTip(controlButtons[i], toolTipMessages[i]);

            toolTipButton.SetToolTip(buttonClearAll, "Полный сброс калькулятора");
            toolTipButton.SetToolTip(labelHelp, "Получить справку по приложению");
        }

        // Обработка нажатий кнопок на форме
        private void buttonForm_Click(object sender, EventArgs e)
        {
            Button but = (Button)sender;
            // Обработка нажатиий на кнопки ввода цифр
            switch (but.Tag)
            {
                // Ввод цифры 0
                case "NumZero":
                    _Control.DoCalculatorCommand(NumCalcCmd.AddZero);
                    break;

                // Ввод цифры 1
                case "NumOne":
                    _Control.DoCalculatorCommand(NumCalcCmd.AddDigit, 1);
                    break;

                // Ввод цифры 2
                case "NumTwo":
                    _Control.DoCalculatorCommand(NumCalcCmd.AddDigit, 2);
                    break;

                // Ввод цифры 3
                case "NumThree":
                    _Control.DoCalculatorCommand(NumCalcCmd.AddDigit, 3);
                    break;

                // Ввод цифры 4
                case "NumFour":
                    _Control.DoCalculatorCommand(NumCalcCmd.AddDigit, 4);
                    break;

                // Ввод цифры 5
                case "NumFive":
                    _Control.DoCalculatorCommand(NumCalcCmd.AddDigit, 5);
                    break;

                // Ввод цифры 6
                case "NumSix":
                    _Control.DoCalculatorCommand(NumCalcCmd.AddDigit, 6);
                    break;

                // Ввод цифры 7
                case "NumSeven":
                    _Control.DoCalculatorCommand(NumCalcCmd.AddDigit, 7);
                    break;

                // Ввод цифры 8
                case "NumEight":
                    _Control.DoCalculatorCommand(NumCalcCmd.AddDigit, 8);
                    break;

                // Ввод цифры 9
                case "NumNine":
                    _Control.DoCalculatorCommand(NumCalcCmd.AddDigit, 9);
                    break;

                // Ввод разделителя
                case "Sep":
                    _Control.DoCalculatorCommand(NumCalcCmd.AddSep);
                    break;

                // Забой символа
                case "Backspace":
                    _Control.DoCalculatorCommand(NumCalcCmd.Backspace);
                    break;

                // Очистка поля редактора
                case "ClearEditor":
                    _Control.DoCalculatorCommand(NumCalcCmd.ClearEditor);
                    break;

                // Сброс калькулятора
                case "ClearAll":
                    if (_Control.DoCalculatorCommand(NumCalcCmd.ReadState) != "WithoutError")
                        for (int i = 0; i < controlButtons.Length; i++) controlButtons[i].Enabled = true;
                    _Control.DoCalculatorCommand(NumCalcCmd.ClearAll);
                    labelOperation.Text = "";
                    break;

                // Смена знака редактируемого числа
                case "Sign":
                    _Control.DoCalculatorCommand(NumCalcCmd.ChangeSign);
                    break;

                // Нахождение обратного числа к редактируемому
                case "FuncRev":
                    _Control.DoCalculatorCommand(NumCalcCmd.Reverse);
                    // Если была попытка найти обратную дробь для дроби с нулевым числителем
                    if (_Control.DoCalculatorCommand(NumCalcCmd.ReadState) == "DivideByZeroError")
                    {
                        // Блокировка клавиш
                        for (int i = 0; i < controlButtons.Length; i++)
                            controlButtons[i].Enabled = false;
                        // Вывод сообщения об исключительной ситуации
                        labelScreen.Text = "Деление на ноль невозможно";
                        return;
                    }
                    break;

                // Возведение редактируемого числа в квадрат
                case "FuncSqr":
                    _Control.DoCalculatorCommand(NumCalcCmd.Square);
                    // Если произошла исключительная ситуация
                    if (_Control.DoCalculatorCommand(NumCalcCmd.ReadState) != "WithoutError")
                    {
                        // Блокировка клавиш
                        for (int i = 0; i < controlButtons.Length; i++)
                            controlButtons[i].Enabled = false;
                        // Вывод собщения об ошибке в зависимости от типа исключительной ситуации
                        labelScreen.Text = (_Control.DoCalculatorCommand(NumCalcCmd.ReadState) == "OverflowError") ? "Слишком большое значение" : "Деление на ноль невозможно";
                        return;
                    }
                    break;

                // Выбор операции сложения
                case "OpAdd":
                    _Control.DoCalculatorCommand(NumCalcCmd.SetAdd);
                    labelOperation.Text = "+";
                    break;

                // Выбор операции вычитания
                case "OpSub":
                    _Control.DoCalculatorCommand(NumCalcCmd.SetSub);
                    labelOperation.Text = "-";
                    break;

                // Выбор операции умножения
                case "OpMul":
                    _Control.DoCalculatorCommand(NumCalcCmd.SetMul);
                    labelOperation.Text = "*";
                    break;

                // Выбор операции деления
                case "OpDiv":
                    _Control.DoCalculatorCommand(NumCalcCmd.SetDiv);
                    labelOperation.Text = "/";
                    break;

                // Вычислить выражениие
                case "EvalExpression":
                    _Control.DoCalculatorCommand(NumCalcCmd.Evaluate);
                    labelOperation.Text = "";
                    // Если произошла исключительная ситуация
                    if (_Control.DoCalculatorCommand(NumCalcCmd.ReadState) != "WithoutError")
                    {
                        // Блокировка клавиш
                        for (int i = 0; i < controlButtons.Length; i++)
                            controlButtons[i].Enabled = false;
                        // Вывод собщения об ошибке в зависимости от типа исключительной ситуации
                        labelScreen.Text = (_Control.DoCalculatorCommand(NumCalcCmd.ReadState) == "OverflowError") ? "Слишком большое значение" : "Деление на ноль невозможно";
                        return;
                    }
                    break;

                // Сохранить значение из редактора в память
                case "MemorySave":
                    _Control.DoCalculatorCommand(NumCalcCmd.MemorySave);
                    // Разблокировка клавииш работы с памятью
                    buttonMemoryClear.Enabled = true;
                    buttonMemoryRead.Enabled = true;
                    labelMemory.Text = "M";
                    break;

                // Добавить сложить значение из редактора и значение из памяти
                case "MemoryAdd":
                    _Control.DoCalculatorCommand(NumCalcCmd.MemoryAdd);
                    // Разблокирование клавиш памяти, если до этого память была выключена
                    if(!buttonMemoryRead.Enabled)
                    {
                        buttonMemoryClear.Enabled = true;
                        buttonMemoryRead.Enabled = true;
                    }
                    labelMemory.Text = "M";
                    break;

                // Чтение значения из память редактора
                case "MemoryRead":
                    _Control.DoCalculatorCommand(NumCalcCmd.MemoryRead);
                    break;

                // Очистка памяти
                case "MemoryClear":
                    _Control.DoCalculatorCommand(NumCalcCmd.MemoryClear);
                    // Блокировка кнопок работы с памятью
                    buttonMemoryClear.Enabled = false;
                    buttonMemoryRead.Enabled = false;
                    labelMemory.Text = "";
                    break;
            }

            labelScreen.Text = _Control.DoCalculatorCommand(NumCalcCmd.ReadEditor);
            buttonEvalExpression.Focus();
        }

        // Обработка нажатиий кнопок на клавиатуре
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Обработка сочетаний клавиш

            // Копирование значения с экрана в буфер обмена
            if (e.KeyCode == Keys.C && e.Control)
                copyContextMenuItem_Click(sender, e);
            // Копирование значения с экрана в буфер обмена и очистка экрана 
            if (e.KeyCode == Keys.X && e.Control)
                cutContextMenuItem_Click(sender, e);
            // Запись значения из буфера обмена на экран
            if (e.KeyCode == Keys.V && e.Control)  
                pasteContextMenuItem_Click(sender, e);

            // Кнопка "плюс" на основной клавиатуре
            if (e.KeyCode == Keys.Oemplus && e.Shift)
                buttonForm_Click(buttonOpAdd, new EventArgs());
            // Кнопка "звездочка" на основной клавиатуре
            else if (e.KeyCode == Keys.D8 && e.Shift)
                buttonForm_Click(buttonOpMul, new EventArgs());
            // Обработка нажатия кнопки клавиатуры
            else if (validKeys.Contains(e.KeyCode))
                validate_KeyDown(e.KeyCode);
            else
                // Нажатие на невалидные клавиши игнорируется
                e.Handled = true;
        }

        // Вызов определенной команды калькулятора в зависимости от нажатой клавиши клавиатуры
        private void validate_KeyDown(Keys KeyCode)
        {
            // Если нажата клавиша с цифрой на основной клавиатуре
            if ((int)KeyCode >= 48 && (int)KeyCode <= 57)
            {
                buttonForm_Click(controlButtons[(int)KeyCode - (int)Keys.D0], new EventArgs());
                return;
            }
            // Если нажата клавиша с цифрой на NumPad
            if ((int)KeyCode >= 96 && (int)KeyCode <= 105)
            {
                buttonForm_Click(controlButtons[(int)KeyCode - (int)Keys.NumPad0], new EventArgs());
                return;
            }

            switch(KeyCode)
            {
                case Keys.Add:
                    // Если нажата клавиша "плюс" на NumPad
                    buttonForm_Click(buttonOpAdd, new EventArgs());
                    return;

                case Keys.Subtract:
                    // Если нажата клавиша "минус" на NumPad
                    buttonForm_Click(buttonOpSub, new EventArgs());
                    return;

                case Keys.Multiply:
                    // Если нажата клавиша "умножить" на NumPad
                    buttonForm_Click(buttonOpMul, new EventArgs());
                    return;

                case Keys.Divide:
                    // Если нажата клавиша "делить" на NumPad
                    buttonForm_Click(buttonOpDiv, new EventArgs());
                    return;

                case Keys.Back:
                    // Если нажата клавиша Backspace
                    buttonForm_Click(buttonBackspace, new EventArgs());
                    return;

                case Keys.Oem2:
                    // Если нажата клавиша "делить" на основной клавиатуре
                    buttonForm_Click(buttonOpDiv, new EventArgs());
                    return;

                case Keys.Oem5:
                    // Если нажата клавиша добавления разделителя
                    buttonForm_Click(buttonSep, new EventArgs());
                    return;

                case Keys.OemMinus:
                    // Если нажата клавиша "минус" на основной клавиатуре
                    buttonForm_Click(buttonOpSub, new EventArgs());
                    return;
            }
        }


        // Обработка пунктов контекстного меню
        // Копировать текст
        private void copyContextMenuItem_Click(object Sender, EventArgs e)
        {
            // Считываем текущее значение из редактора
            _ClipBoard = _Control.DoCalculatorCommand(NumCalcCmd.ReadEditor);
        }
            
       // Вставить текст
       private void pasteContextMenuItem_Click(object Sender, EventArgs e)
       {
            // Записываем в редактор значениие из буфера
            if (_ClipBoard != System.String.Empty)
            {
                _Control.DoCalculatorCommand(NumCalcCmd.ClearEditor);
                labelScreen.Text = _Control.DoCalculatorCommand(NumCalcCmd.WriteEditor, _ClipBoard);
            }
       }

       // Вырезать текст
       private void cutContextMenuItem_Click(object Sender, EventArgs e)
       {
            // Считываем текущее значение из редактора
            _ClipBoard = _Control.DoCalculatorCommand(NumCalcCmd.ReadEditor);
            // Очищаем поле редактора
            labelScreen.Text = _Control.DoCalculatorCommand(NumCalcCmd.ClearEditor);
        }

        // Показ окна со справкой по приложению
        private void labelHelp_Click(object sender, EventArgs e)
        {
            var helpForm = new HelpForm();
            helpForm.Show();
        }
    }
}