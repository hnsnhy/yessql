using Dapper;
using System.Data.Common;
using System.Threading.Tasks;
using YesSql.Collections;
using YesSql.Logging;

namespace YesSql.Commands
{
    public class CreateDocumentCommand : DocumentCommand
    {
        private readonly string _tablePrefix;

        public override int ExecutionOrder { get; } = 0;

        public CreateDocumentCommand(Document document, string tablePrefix) : base(document)
        {
            _tablePrefix = tablePrefix;
        }

        public override Task ExecuteAsync(DbConnection connection, DbTransaction transaction, ISqlDialect dialect, ILogger logger)
        {
            var documentTable = CollectionHelper.Current.GetPrefixedName(Store.DocumentTable);
            var insertCmd = "insert into " + dialect.QuoteForTableName(_tablePrefix + documentTable) + " (" + dialect.QuoteForColumnName("Id") + ", " + dialect.QuoteForColumnName("Type") + ", " + dialect.QuoteForColumnName("Content") + ") values (@Id, @Type, @Content);";

            logger.LogSql(insertCmd);

            return connection.ExecuteScalarAsync<int>(insertCmd, Document, transaction);
        }
    }
}
