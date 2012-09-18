using COAT.Schedule;
using COAT.Data.Import;
using COAT.IDS;
using System.Net.Mail;
using COAT.Helper;

namespace COAT.Models
{
    public partial class COATImportTask : IScheduleTask
    {
        public struct TaskType
        {
            public const string ImportORPDeal = "ImpotORPDeal";
            public const string ImportExcutiveDeal = "ImportExcutiveDeal";
            public const string ImportNameAccountList = "ImportNameAccountList";
            public const string ImportChannelAssigner = "ImportChannelAssigner";
            public const string ImportChannelApprover = "ImportChannelApprover";
            public const string ImportChannelDirector = "ImportChannelDirector";
            public const string ImportSalesAssigner = "ImportSalesAssigner";
            public const string ImportSalesApprover = "ImportSalesApprover";
            public const string ImportSalesDirector = "ImportSalesDirector";
            public const string ImportVisitor = "ImportVisitor";
            public const string ImportNameAccountSales = "ImportNameAccountSales";
            public const string ImportVolumeSales = "ImportVolumeSales";
        }

        public const string ImportTaskName = "ImportTask";

        public COATImportTask()
        {
            ImportTask = new ImportTask();
            Name = ImportTaskName;
        }

        public COATImportTask(ImportTask task)
        {
            ImportTask = task;
            Name = ImportTaskName;
        }

        public ImportTask ImportTask { get; private set; }

        public int RunTimes
        { get; set; }

        public void Run(object obj)
        {
            if (string.IsNullOrWhiteSpace(this.Type))
                return;

            switch (Type)
            {
                case TaskType.ImportORPDeal:
                    new DealImportHelper(FilePath).ImportRawData();
                    break;
                case TaskType.ImportExcutiveDeal:
                    new ExcutivedDealImportHelper(FilePath).ImportRawData();
                    break;
                case TaskType.ImportNameAccountList:
                    new NameAccountImporttHelper(FilePath).ImportRawData();
                    break;
                case TaskType.ImportChannelAssigner:
                    GetImportHelper(SystemRoleIds.ChannelAssigner, BusinessRoleIds.ChannelManager, FilePath).ImportRawData();
                    break;
                case TaskType.ImportChannelApprover:
                    GetImportHelper(SystemRoleIds.ChannelApprover, BusinessRoleIds.ChannelManager, FilePath).ImportRawData();
                    break;
                case TaskType.ImportChannelDirector:
                    GetImportHelper(SystemRoleIds.ChannelDirector, BusinessRoleIds.ChannelDirector, FilePath).ImportRawData();
                    break;
                case TaskType.ImportSalesAssigner:
                    GetImportHelper(SystemRoleIds.SalesAssigner, BusinessRoleIds.InsideSales, FilePath).ImportRawData();
                    break;
                case TaskType.ImportSalesApprover:
                    GetImportHelper(SystemRoleIds.SalesApprover, BusinessRoleIds.InsideSales, FilePath).ImportRawData();
                    break;
                case TaskType.ImportSalesDirector:
                    GetImportHelper(SystemRoleIds.SalesDirector, BusinessRoleIds.SalesDirector, FilePath).ImportRawData();
                    break;
                case TaskType.ImportVisitor:
                    GetImportHelper(SystemRoleIds.Visitor, BusinessRoleIds.Unkonwn, FilePath).ImportRawData();
                    break;
                case TaskType.ImportNameAccountSales:
                    GetImportHelper(SystemRoleIds.Forbidden, BusinessRoleIds.NameAccountSales, FilePath).ImportRawData();
                    break;
                case TaskType.ImportVolumeSales:
                    GetImportHelper(SystemRoleIds.Forbidden, BusinessRoleIds.VolumeSales, FilePath).ImportRawData();
                    break;
            }
        }

        public void Success()
        {
            ImportTask.IsComplete = true;
        }

        public void Error(string msg)
        {
            if (this.RunTimes > 3)
                ImportTask.IsComplete = true;

            this.ErrorMessage = msg;
        }


        public int Id
        {
            get
            {
                return ImportTask.Id;
            }
            set
            {
                ImportTask.Id = value;
            }
        }

        public string Name { get; set; }


        public string Type
        {
            get
            {
                return ImportTask.Type;
            }
            set
            {
                ImportTask.Type = value;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return ImportTask.ErrorMessage;
            }
            set
            {
                ImportTask.ErrorMessage = value;
            }
        }

        public string FilePath { get { return ImportTask.FilePath; } set { ImportTask.FilePath = value; } }

        private UserImportHelper GetImportHelper(int sysRoleId, int busRoleId, string path)
        {
            var msg = GetMailMessage();
            return new UserImportHelper(sysRoleId, busRoleId, null, path);
        }

        private MailMessage GetMailMessage()
        {
            MailMessage msg = new MailMessage();
            msg.Subject = "COAT Portal Account Notification Email";
            msg.Body = new MailTempleteHepler().GetTemplete(MailTempleteHepler.NewAccount, "{0}/{1}");
            return msg;
        }

    }
}