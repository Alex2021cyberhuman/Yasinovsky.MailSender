using System;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Yasinovsky.MailSender.Services.Wpf.ViewModels
{
    internal class MessageViewModel : ObservableObject
    {
        [Obsolete("Not work! Designer only ctor.")]
        public MessageViewModel()
        {
            Caption = "caption";
            Message = "Very long text\nLorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн. Его популяризации в новое время послужили публикация листов Letraset с образцами Lorem Ipsum в 60-х годах и, в более недавнее время, программы электронной вёрстки типа Aldus PageMaker, в шаблонах которых используется Lorem Ipsum.";
            Status = 2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="message"></param>
        /// <param name="status">
        /// 0 - Information,
        /// 1 - Warning,
        /// 2 - Error
        /// </param>
        public MessageViewModel(string caption, string message, int status = 0)
        {
            Caption = caption;
            Message = message;
            Status = status;
            OkCommand = new RelayCommand(() => OnDialogReturn(true));
        }
        
        public event EventHandler<bool?> DialogReturn;

        public string Caption { get; set; }

        public string Message { get; set; }

        public int Status { get; set; }

        public ICommand OkCommand { get; }

        protected virtual void OnDialogReturn(bool? e)
        {
            DialogReturn?.Invoke(this, e);
        }
    }
}
