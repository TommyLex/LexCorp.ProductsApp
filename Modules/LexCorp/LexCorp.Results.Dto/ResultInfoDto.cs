using System.Collections.Generic;
using System.Linq;

namespace LexCorp.Results.Dto
{
  /// <summary>
  /// General structure for communicating the execution status of an operation.
  /// </summary>
  public class ResultInfoDto
  {
    /// <summary>
    /// The status of the result.
    /// </summary>
    public virtual bool Success { get; set; }

    /// <summary>
    /// Messages for the user.
    /// </summary>
    public virtual IList<string> Messages { get; set; } = new List<string>();

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultInfoDto"/> class.
    /// </summary>
    public ResultInfoDto()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultInfoDto"/> class with the specified success status and messages.
    /// </summary>
    /// <param name="success">The success status of the operation.</param>
    /// <param name="messages">The messages to include in the result.</param>
    public ResultInfoDto(bool success, params string[] messages)
    {
      Success = success;
      _AddMessages(messages);
    }

    /// <summary>
    /// Combines the current result with a new one.
    /// Messages will be merged, and success will be set to false if at least one of the results is false.
    /// </summary>
    /// <param name="toAdd">The result to add to the current one.</param>
    public void Concat(ResultInfoDto toAdd)
    {
      if (!Success || !toAdd.Success)
        Success = false;

      _AddMessages(toAdd.Messages.ToArray());
    }

    /// <summary>
    /// Adds messages to the <see cref="Messages"/> collection.
    /// </summary>
    /// <param name="messages">The messages to add.</param>
    private void _AddMessages(params string[] messages)
    {
      foreach (string m in messages)
      {
        Messages.Add(m);
      }
    }

    /// <summary>
    /// Allows direct assignment of a boolean value to a <see cref="ResultInfoDto"/> object.
    /// </summary>
    /// <param name="value">A boolean value. This value will be assigned to <see cref="ResultInfoDto.Success"/>.</param>
    public static implicit operator ResultInfoDto(bool value)
    {
      return new ResultInfoDto(value);
    }

    /// <summary>
    /// Allows direct assignment of an error message to a <see cref="ResultInfoDto"/> object. Sets <see cref="Success"/> to false.
    /// </summary>
    /// <param name="value">The error message to include in <see cref="ResultInfoDto.Messages"/>.</param>
    public static implicit operator ResultInfoDto(string value)
    {
      return new ResultInfoDto(false, value);
    }
  }

  /// <summary>
  /// General structure for communicating the execution status of an operation.
  /// Variant for passing data.
  /// </summary>
  /// <typeparam name="T">The type of the data to include in the result.</typeparam>
  public class ResultInfoDto<T> : ResultInfoDto
  {
    /// <summary>
    /// The data included in the result.
    /// </summary>
    public T Data { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultInfoDto{T}"/> class.
    /// </summary>
    public ResultInfoDto() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultInfoDto{T}"/> class with the specified data.
    /// Sets <see cref="ResultInfoDto.Success"/> to true.
    /// </summary>
    /// <param name="data">The data to include in the result.</param>
    public ResultInfoDto(T data) : base(true)
    {
      Data = data;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultInfoDto{T}"/> class with the specified error messages.
    /// Sets <see cref="ResultInfoDto.Success"/> to false.
    /// </summary>
    /// <param name="errorMessages">The error messages to include in the result.</param>
    public ResultInfoDto(params string[] errorMessages) : base(false, errorMessages)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultInfoDto{T}"/> class with the specified success status, data, and messages.
    /// </summary>
    /// <param name="success">The success status of the operation.</param>
    /// <param name="data">The data to include in the result.</param>
    /// <param name="messages">The messages to include in the result.</param>
    public ResultInfoDto(bool success, T data, params string[] messages) : base(success, messages)
    {
      Data = data;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultInfoDto{T}"/> class based on an existing <see cref="ResultInfoDto"/> and data.
    /// </summary>
    /// <param name="resultInfo">The existing result information.</param>
    /// <param name="data">The data to include in the result.</param>
    public ResultInfoDto(ResultInfoDto resultInfo, T data) : this(resultInfo.Success, data, resultInfo.Messages.ToArray())
    {
    }

    /// <summary>
    /// Allows direct assignment of data to a <see cref="ResultInfoDto{T}"/> object. Sets <see cref="ResultInfoDto.Success"/> to true.
    /// </summary>
    /// <param name="value">The data to include in <see cref="ResultInfoDto{T}.Data"/>.</param>
    public static implicit operator ResultInfoDto<T>(T value)
    {
      return new ResultInfoDto<T>(value);
    }

    /// <summary>
    /// Allows direct assignment of an error message to a <see cref="ResultInfoDto{T}"/> object. Sets <see cref="ResultInfoDto.Success"/> to false.
    /// </summary>
    /// <param name="value">The error message to include in <see cref="ResultInfoDto.Messages"/>.</param>
    public static implicit operator ResultInfoDto<T>(string value)
    {
      return new ResultInfoDto<T>(value);
    }
  }
}