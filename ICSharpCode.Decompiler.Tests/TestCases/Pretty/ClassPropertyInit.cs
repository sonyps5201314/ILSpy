using System;
using System.Diagnostics;
using System.Threading;

namespace ClassPropertyInit
{
	public record struct CopilotContextId
	{
		public Guid Id { get; }

		public CopilotContextId()
		{
			this.Id = Guid.NewGuid();
		}

		public CopilotContextId(Guid id) => this.Id = id;
	}

	public class CopilotContextId_Class(Guid id)
	{
		public Guid guid { get; } = id;

		public CopilotContextId_Class(Guid id, int value) : this(Guid.NewGuid())
		{

		}
		public CopilotContextId_Class() : this(Guid.NewGuid(), 222)
		{

		}
	}

	public record CopilotContextId_Record(Guid id)
	{
		public Guid guid { get; } = id;

		public CopilotContextId_Record() : this(Guid.NewGuid())
		{

		}
	}


	public record class CopilotContextId_RecordClass(Guid id)
	{
		public Guid guid { get; } = id;

		public CopilotContextId_RecordClass() : this(Guid.NewGuid())
		{

		}
	}


	public record struct CopilotContextId_RecordStruct(Guid id)
	{
		public Guid guid { get; } = id;

		public CopilotContextId_RecordStruct() : this(Guid.NewGuid())
		{

		}
	}


	public struct CopilotContextId_Struct(Guid id)
	{
		public Guid guid { get; } = id;

		public CopilotContextId_Struct() : this(Guid.NewGuid())
		{

		}
	}

	/// <summary>
	/// Represents a mention that can be displayed or queried later.
	/// </summary>
	/// <seealso cref="M:Microsoft.VisualStudio.Copilot.ICopilotMentionQueryable.QueryMentionAsync(Microsoft.VisualStudio.Copilot.CopilotMentionQuery,System.Threading.CancellationToken)" />
	public abstract record CopilotQueriedMention
	{


		/// <summary>Gets the name to display for this query item.</summary>
		/// <example>
		/// 1. File.cs:15-20.
		/// 2. Exception
		/// </example>
		/// <remarks>
		/// The display name for a mentioned item should either represent the name/member of the mentionable being
		/// referenced (ex. exception) or the identifier for a particular set of inputs to a member (ex. File.cs).
		/// As such, the display name should not be localized as it is either the same name as a user recognizes,
		/// or it will match what is inserted into the prompt.
		/// </remarks>
		public string DisplayName { get; init; }

		/// <summary>
		/// Gets the full string to be inserted into the prompt, minus trigger characters.
		/// </summary>
		/// <remarks>
		/// The string here must be fully parseable to the exact context.
		/// </remarks>
		public string FullName { get; init; }

		/// <summary>
		/// Gets a small additional bit of description to display to the user.
		/// </summary>
		/// <example>Current document.</example>
		public string? Description { get; init; }

		/// <summary>
		/// Gets optional more detailed information to display to the user in a tooltip when selected in the completions list,
		/// otherwise the display name plus the description is shown.
		/// </summary>
		/// <remarks>
		/// This value is displayed to the user, so ensure it is appropriately localized.
		/// </remarks>
		public string? Tooltip { get; init; }


		/// <summary>
		/// Gets a value indicating whether the referenced mention can be navigated to/opened.
		/// </summary>
		/// <seealso cref="M:Microsoft.VisualStudio.Copilot.ICopilotMentionQueryable.NavigateToMentionableAsync(Microsoft.VisualStudio.Copilot.CopilotQueriedMention,System.Threading.CancellationToken)" />
		public bool IsNavigable { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Microsoft.VisualStudio.Copilot.CopilotQueriedMention" /> class.
		/// </summary>
		/// <param name="providerMoniker">The provider moniker providing this mention.</param>
		/// <param name="fullName">The full expanded parseable string without trigger characters.</param>
		/// <param name="displayName">The display name.</param>
		internal CopilotQueriedMention(
		  string fullName,
		  string displayName)
		{
			this.FullName = fullName;
			this.DisplayName = displayName;
		}
	}


	/// <summary>
	/// Represents a query into a scope.
	/// </summary>
	/// <seealso cref="T:Microsoft.VisualStudio.Copilot.ICopilotScope" />
	public record CopilotQueriedScopeMention : CopilotQueriedMention
	{
		/// <inheritdoc />
		public string Type { get; } = "Context";

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Microsoft.VisualStudio.Copilot.CopilotQueriedScopeMention" /> class.
		/// </summary>
		/// <param name="providerMoniker"></param>
		/// <param name="fullName"></param>
		/// <param name="displayName"></param>
		public CopilotQueriedScopeMention(string fullName, string displayName)
			: base(fullName, displayName)
		{
		}
	}

	public class DeserializationException(string response, Exception innerException) : Exception("Error occured while deserializing the response", innerException)
	{
		public string Response { get; } = response;
	}

	static class Ensure
	{
		public static T NotNull<T>(T? value, string name) => value ?? throw new ArgumentNullException(name);

		public static string NotEmptyString(object? value, string name)
		{
			var s = value as string ?? value?.ToString();
			if (s == null)
				throw new ArgumentNullException(name);

			return string.IsNullOrWhiteSpace(s) ? throw new ArgumentException("Parameter cannot be an empty string", name) : s;
		}
	}

	public struct FromBinaryOperator
	{
		public int Leet;

		public FromBinaryOperator(int dummy1, int dummy2)
		{
			Leet = dummy1 + dummy2;
		}
	}

	public struct FromCall
	{
		public int Leet;

		public FromCall(int dummy1, int dummy2)
		{
			Leet = Math.Max(dummy1, dummy2);
		}
	}

	public struct FromConvert
	{
		public int Leet;

		public FromConvert(double dummy1, double dummy2)
		{
			Leet = (int)Math.Min(dummy1, dummy2);
		}
	}


	public record NamedParameter(string name, object? value, bool encode = true) : Parameter(Ensure.NotEmptyString(name, nameof(name)), value, encode)
	{
	}

	/// <summary>
	/// Parameter container for REST requests
	/// </summary>
	[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}()}}")]
	public abstract record Parameter
	{



		/// <summary>
		/// Parameter name
		/// </summary>
		public string? Name { get; }

		/// <summary>
		/// Parameter value
		/// </summary>
		public object? Value { get; }


		/// <summary>
		/// Indicates if the parameter value should be encoded or not.
		/// </summary>
		public bool Encode { get; }

		/// <summary>
		/// Return a human-readable representation of this parameter
		/// </summary>
		/// <returns>String</returns>
		public sealed override string ToString() => Value == null ? $"{Name}" : $"{Name}={ValueString}";

		protected virtual string ValueString => Value?.ToString() ?? "null";

		/// <summary>
		/// Parameter container for REST requests
		/// </summary>
		protected Parameter(string? name, object? value, bool encode)
		{
			Name = name;
			Value = value;
			Encode = encode;
		}

		/// <summary>
		/// Assists with debugging by displaying in the debugger output
		/// </summary>
		/// <returns></returns>
		protected string DebuggerDisplay() => $"{GetType().Name.Replace("Parameter", "")} {ToString()}";
	}


	public class Person(string name, int age)
	{
		private readonly string _name = name;
		private readonly int _age = age;

		public string Email { get; init; }

		// 这个构造函数会调用主构造函数
		public Person(string name, int age, string email) : this(name, age)
		{
			// 这里可以有函数体
			if (string.IsNullOrEmpty(email))
				throw new ArgumentException("Email cannot be empty");
			Email = email;
			Console.WriteLine($"Created person: {name}");
		}

	}



	public class PersonPrimary(string name, int age)
	{
		private readonly string _name = name;
	}

	public class PersonPrimary_CaptureParams(string name, int age)
	{
		public string GetDetails() => $"{name}, {age}";
	}

	public class PersonRegular1
	{
		private readonly string _name = "name";
		private readonly int _age = 23;

		public PersonRegular1(string name, int age)
		{
			Thread.Sleep(1000);
			_age = name.Length;
		}
	}

	public class PersonRegular2
	{
		private readonly string _name = "name" + Environment.GetEnvironmentVariable("Path");
		private readonly int _age = Environment.GetEnvironmentVariable("Path")?.Length ?? -1;

		public PersonRegular2(string name, int age)
		{
			//Thread.Sleep(1000);
			//_age = name.Length;
		}
	}


	public record QueryParameter(string name, object? value, bool encode = true) : NamedParameter(name, value, encode);


	struct Range
	{
		/// <summary>Represent the inclusive start index of the Range.</summary>
		public object Start { get; }

		/// <summary>Represent the exclusive end index of the Range.</summary>
		public object End { get; }

		/// <summary>Construct a Range object using the start and end indexes.</summary>
		/// <param name="start">Represent the inclusive start index of the range.</param>
		/// <param name="end">Represent the exclusive end index of the range.</param>
		public Range(object start, object end)
		{
			Start = start;
			End = end;
		}
	}

	ref struct RefFields
	{
		public ref int Field0;
		public RefFields(ref int v)
		{
			Field0 = ref v;
		}
	}

	struct StructWithDefaultCtor
	{
		int X = 42;
		public StructWithDefaultCtor()
		{

		}
	}




	public ref struct TestRef(ref int a)
	{
		public ref int AAA = ref a;

		public void Print()
		{
			int val = AAA;
			Console.WriteLine(val);
		}
	}

	struct ValueFields
	{
		public int Field0;
		public ValueFields(int v)
		{
			Field0 = v;
		}
	}

	class WebPair1(string name)
	{
		public string Name { get; } = name;
	}

	class WebPair2(string name, string? value, ref readonly object encode)
	{
		public string Name { get; } = name;
	}

	class WebPair3(string name, string? value, bool encode = false)
	{
		public string Name { get; } = name;
		public string? Value { get; } = value;
		string? WebValue { get; } = encode ? "111" : value;
		//string? WebValue { get; } = "111" + value;
	}

	class WebPair4(string name, string? value, ref readonly object encode)
	{
		public string Name { get; } = name;
		public string? Value { get; } = value;
		string? WebValue { get; } = encode is null ? "111" : value;
		string? WebValue2 { get; } = encode.ToString();
	}

	class WebPair5(string name, string? value)
	{
		public string Name { get; } = name;
	}

	class WebPair6(string name, string? value, ref readonly object encode)
	{
		public string? Value { get; } = name;
		public string Name { get; } = value;
		string? WebValue { get; } = name != null ? "111" : value;
		string? WebValue2 { get; } = value != null ? name : "222";
	}
}
