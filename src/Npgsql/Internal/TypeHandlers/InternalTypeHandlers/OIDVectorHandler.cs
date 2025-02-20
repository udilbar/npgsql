﻿using System;
using Npgsql.Internal.TypeHandlers.NumericHandlers;
using Npgsql.Internal.TypeHandling;
using Npgsql.PostgresTypes;
using Npgsql.TypeMapping;
using NpgsqlTypes;

namespace Npgsql.Internal.TypeHandlers.InternalTypeHandlers
{
    class OIDVectorHandlerFactory : NpgsqlTypeHandlerFactory
    {
        public override NpgsqlTypeHandler CreateNonGeneric(PostgresType pgType, NpgsqlConnector conn)
            => new OIDVectorHandler(pgType, conn.TypeMapper.DatabaseInfo.ByName["oid"]
                                    ?? throw new NpgsqlException("Two types called 'oid' defined in the database"));

        public override Type DefaultValueType => typeof(uint[]);
    }

    /// <summary>
    /// An OIDVector is simply a regular array of uints, with the sole exception that its lower bound must
    /// be 0 (we send 1 for regular arrays).
    /// </summary>
    class OIDVectorHandler : ArrayHandler<uint>
    {
        public OIDVectorHandler(PostgresType oidvectorType, PostgresType oidType)
            : base(oidvectorType, new UInt32Handler { PostgresType = oidType }, ArrayNullabilityMode.Never, 0) { }

        public override ArrayHandler CreateArrayHandler(PostgresArrayType pgArrayType, ArrayNullabilityMode arrayNullabilityMode)
            => new ArrayHandler<ArrayHandler<uint>>(pgArrayType, this, arrayNullabilityMode);
    }
}
