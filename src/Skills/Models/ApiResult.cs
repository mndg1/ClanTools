namespace Skills.Models;

internal record ApiResult<T> (bool Successful, T Result);
