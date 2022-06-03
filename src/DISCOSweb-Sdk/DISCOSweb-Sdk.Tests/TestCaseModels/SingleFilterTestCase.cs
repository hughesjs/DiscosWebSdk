using System;
using DISCOSweb_Sdk.Enums;

namespace DISCOSweb_Sdk.Tests.TestCaseModels;

public struct SingleFilterTestCase
{
	public SingleFilterTestCase(Type objectType, Type paramType, string paramName, object? paramValue, DiscosFunction func, string expected, int testNum = 0)
	{
		ObjectType = objectType;
		ParamType = paramType;
		ParamName = paramName;
		ParamValue = paramValue;
		Func = func;
		Expected = expected;
		TestNum = testNum;
	}

	public DiscosFunction Func { get; }
	public string Expected { get; }
	public string ParamName { get; }
	public object? ParamValue { get; }
	public Type ParamType { get; }
	public Type ObjectType { get; }
	public int TestNum { get; }
}
