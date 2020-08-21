using System;
using System.Collections.Generic;

namespace HotChocolate.Internal
{
    public interface ITypeInfo
    {
        /// <summary>
        /// Gets the type component that represents the named type.
        /// </summary>
        Type NamedType { get; }

        /// <summary>
        /// Gets the original type from which this type info was inferred.
        /// </summary>
        Type OriginalType { get; }

        /// <summary>
        /// The components represent the GraphQL type structure.
        /// </summary>
        IReadOnlyList<TypeComponentKind> Components { get; }

        /// <summary>
        /// Defines if the <see cref="NamedType"/> is a GraphQL schema type.
        /// </summary>
        bool IsSchemaType { get; }

        /// <summary>
        /// Defines if the <see cref="NamedType"/> is a runtime type.
        /// </summary>
        bool IsRuntimeType { get; }

        /// <summary>
        /// Gets the extended type that contains information
        /// about type arguments and nullability.
        /// </summary>
        IExtendedType GetExtendedType();
    }
}