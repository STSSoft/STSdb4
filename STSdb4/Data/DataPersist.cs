using STSdb4.General.Persist;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Reflection;

namespace STSdb4.Data
{
    public class DataPersist : IPersist<IData>
    {
        public readonly Action<BinaryWriter, IData> write;
        public readonly Func<BinaryReader, IData> read;

        public readonly Type Type;
        public readonly Func<Type, MemberInfo, int> MembersOrder;
        public readonly AllowNull AllowNull;

        public DataPersist(Type type, Func<Type, MemberInfo, int> membersOrder = null, AllowNull allowNull = AllowNull.None)
        {
            Type = type;
            MembersOrder = membersOrder;
            AllowNull = allowNull;

            write = CreateWriteMethod().Compile();
            read = CreateReadMethod().Compile();
        }

        public void Write(BinaryWriter writer, IData item)
        {
            write(writer, item);
        }

        public IData Read(BinaryReader reader)
        {
            return read(reader);
        }

        public Expression<Action<BinaryWriter, IData>> CreateWriteMethod()
        {
            var writer = Expression.Parameter(typeof(BinaryWriter), "writer");
            var idata = Expression.Parameter(typeof(IData), "idata");

            var dataType = typeof(Data<>).MakeGenericType(Type);
            var dataValue = Expression.Variable(Type, "dataValue");

            var assign = Expression.Assign(dataValue, Expression.Convert(idata, dataType).Value());

            return Expression.Lambda<Action<BinaryWriter, IData>>(Expression.Block(new ParameterExpression[] { dataValue }, assign, PersistHelper.CreateWriteBody(dataValue, writer, MembersOrder, AllowNull)), writer, idata);
        }

        public Expression<Func<BinaryReader, IData>> CreateReadMethod()
        {
            var reader = Expression.Parameter(typeof(BinaryReader), "reader");

            var dataType = typeof(Data<>).MakeGenericType(Type);

            return Expression.Lambda<Func<BinaryReader, IData>>(
                    Expression.Label(Expression.Label(dataType), Expression.New(dataType.GetConstructor(new Type[] { Type }), PersistHelper.CreateReadBody(reader, Type, MembersOrder, AllowNull))),
                    reader
                );
        }
    }
}