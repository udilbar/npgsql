﻿using System;
using Npgsql.Internal.TypeHandlers.NumericHandlers;
using Npgsql.Internal.TypeHandling;
using Npgsql.PostgresTypes;
using Npgsql.TypeMapping;
using NpgsqlTypes;

namespace Npgsql.Internal.TypeHandlers.InternalTypeHandlers
{
    class Int2VectorHandlerFactory : NpgsqlTypeHandlerFactory
    {
        public override NpgsqlTypeHandler CreateNonGeneric(PostgresType pgType, NpgsqlConnector conn)
            => new Int2VectorHandler(pgType, conn.TypeMapper.DatabaseInfo.ByName["smallint"]
                                             ?? throw new NpgsqlException("Two types called 'smallint' defined in the database"));

        public override Type DefaultValueType => typeof(short[]);
    }

    /// <summary>
    /// An int2vector is simply a regular array of shorts, with the sole exception that its lower bound must
    /// be 0 (we send 1 for regular arrays).
    /// </summary>
    class Int2VectorHandler : ArrayHandler<short>
    {
        public Int2VectorHandler(PostgresType arrayPostgresType, PostgresType postgresShortType)
            : base(arrayPostgresType, new Int16Handler { PostgresType = postgresShortType }, ArrayNullabilityMode.Never, 0) { }

        public override ArrayHandler CreateArrayHandler(PostgresArrayType pgArrayType, ArrayNullabilityMode arrayNullabilityMode)
            => new ArrayHandler<ArrayHandler<short>>(pgArrayType, this, arrayNullabilityMode);
    }
}
