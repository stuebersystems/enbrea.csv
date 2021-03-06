﻿#region ENBREA.CSV - Copyright (C) 2021 STÜBER SYSTEMS GmbH
/*    
 *    ENBREA.CSV 
 *    
 *    Copyright (C) 2021 STÜBER SYSTEMS GmbH
 *
 *    Licensed under the MIT License, Version 2.0. 
 * 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace Enbrea.Csv
{
    /// <summary>
    /// A CSV table writer which generates CSV data line by line
    /// </summary>
    public class CsvLineTableWriter : CsvTableAccess
    {
        private readonly CsvLineBuilder _csvLineBuilder;
        private string[] _csvValues = new string[0];

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvLineTableWriter"/> class.
        /// </summary>
        /// <param name="csvLineBuilder">The <see cref="CsvLineBuilder"/> as string generator</param>
        public CsvLineTableWriter(CsvLineBuilder csvLineBuilder)
            : base()
        {
            _csvLineBuilder = csvLineBuilder;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvLineTableWriter"/> class.
        /// </summary>
        /// <param name="csvLineBuilder">The <see cref="CsvLineBuilder"/> as string generator</param>
        /// <param name="csvHeaders">List of csv headers</param>
        public CsvLineTableWriter(CsvLineBuilder csvLineBuilder, CsvHeaders csvHeaders)
            : base(csvHeaders)
        {
            _csvLineBuilder = csvLineBuilder;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvLineTableWriter"/> class.
        /// </summary>
        /// <param name="csvLineBuilder">The <see cref="CsvLineBuilder"/> as string generator</param>
        /// <param name="csvHeaders">List of csv headers</param>
        public CsvLineTableWriter(CsvLineBuilder csvLineBuilder, params string[] csvHeaders)
            : this(csvLineBuilder, new CsvHeaders(csvHeaders))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvLineTableWriter"/> class.
        /// </summary>
        /// <param name="csvLineBuilder">The <see cref="CsvLineBuilder"/> as string generator</param>
        /// <param name="csvHeaders">List of csv headers</param>
        public CsvLineTableWriter(CsvLineBuilder csvLineBuilder, IList<string> csvHeaders)
            : this(csvLineBuilder, new CsvHeaders(csvHeaders))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvLineTableWriter"/> class.
        /// </summary>
        /// <param name="csvLineBuilder">The <see cref="CsvLineBuilder"/> as string generator</param>
        /// <param name="csvConverterResolver">Your own implementation of a value converter resolver</param>
        public CsvLineTableWriter(CsvLineBuilder csvLineBuilder, ICsvConverterResolver csvConverterResolver)
            : base(csvConverterResolver)
        {
            _csvLineBuilder = csvLineBuilder;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvLineTableWriter"/> class.
        /// </summary>
        /// <param name="csvLineBuilder">The <see cref="CsvLineBuilder"/> as string generator</param>
        /// <param name="csvConverterResolver">Your own implementation of a value converter resolver</param>
        /// <param name="csvHeaders">List of csv headers</param>
        public CsvLineTableWriter(CsvLineBuilder csvLineBuilder, ICsvConverterResolver csvConverterResolver, CsvHeaders csvHeaders)
            : base(csvHeaders, csvConverterResolver)
        {
            _csvLineBuilder = csvLineBuilder;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvLineTableWriter"/> class.
        /// </summary>
        /// <param name="csvLineBuilder">The <see cref="CsvLineBuilder"/> as string generator</param>
        /// <param name="csvConverterResolver">Your own implementation of a value converter resolver</param>
        /// <param name="csvHeaders">List of csv headers</param>
        public CsvLineTableWriter(CsvLineBuilder csvLineBuilder, ICsvConverterResolver csvConverterResolver, params string[] csvHeaders)
            : this(csvLineBuilder, csvConverterResolver, new CsvHeaders(csvHeaders))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvLineTableWriter"/> class.
        /// </summary>
        /// <param name="csvLineBuilder">The <see cref="CsvLineBuilder"/> as string generator</param>
        /// <param name="csvConverterResolver">Your own implementation of a value converter resolver</param>
        /// <param name="csvHeaders">List of csv headers</param>
        public CsvLineTableWriter(CsvLineBuilder csvLineBuilder, ICsvConverterResolver csvConverterResolver, IList<string> csvHeaders)
            : this(csvLineBuilder, csvConverterResolver, new CsvHeaders(csvHeaders))
        {
        }

        /// <summary>
        /// Gets and sets the value of the current csv record at the specified index.
        /// </summary>
        /// <param name="i">Index of the value</param>
        /// <returns>A string value</returns>
        public string this[int i]
        {
            get
            {
                return _csvValues[i];
            }
            set
            {
                _csvValues[i] = value;
            }
        }

        /// <summary>
        /// Gets and sets the value of the current csv record at the posiiton of the specified header name.
        /// </summary>
        /// <param name="name">Name of the csv header</param>
        /// <returns>A string value</returns>
        public string this[string name]
        {
            get
            {
                var i = Headers.IndexOf(x => x == name);
                if (i != -1)
                {
                    return _csvValues[i];
                }
                else
                {
                    throw new CsvHeaderNotFoundException($"CSV Header \"{name}\" not found");
                }
            }
            set
            {
                var i = Headers.IndexOf(x => x == name);
                if (i != -1)
                {
                    _csvValues[i] = value;
                }
                else
                {
                    throw new CsvHeaderNotFoundException($"CSV Header \"{name}\" not found");
                }
            }
        }

        /// <summary>
        /// Sets the value of the current csv record at the posiiton of the specified header name.
        /// </summary>
        /// <param name="name">Name of a header</param>
        /// <param name="value">A value</param>
        public void SetValue(string name, string value)
        {
            this[name] = value;
        }

        /// <summary>
        /// Sets the value of the current csv record at the specified index. 
        /// </summary>
        /// <param name="index">Index within the current csv record</param>
        /// <param name="value">A value</param>
        public void SetValue(int index, string value)
        {
            this[index] = value;
        }

        /// <summary>
        /// Sets the value of the current csv record at the posiiton of the specified header name.
        /// </summary>
        /// <param name="name">Name of a header</param>
        /// <param name="value">A value</param>
        public void SetValue<T>(string name, T value)
        {
            SetValue(name, value, ConverterResolver.GetConverter<T>());
        }

        /// <summary>
        /// Sets the value of the current csv record at the posiiton of the specified header name.
        /// </summary>
        /// <param name="name">Name of a header</param>
        /// <param name="value">A value</param>
        public void SetValue<T>(string name, T value, ICsvConverter valueConverter)
        {
            this[name] = valueConverter.ToString(value);
        }

        /// <summary>
        /// Sets the value of the current csv record at the specified index. 
        /// </summary>
        /// <param name="index">Index within the current csv record</param>
        /// <param name="value">A value</param>
        public void SetValue<T>(int index, T value)
        {
            SetValue(index, value, ConverterResolver.GetConverter<T>());
        }

        /// <summary>
        /// Sets the value of the current csv record at the specified index. 
        /// </summary>
        /// <param name="index">Index within the current csv record</param>
        /// <param name="value">A value</param>
        public void SetValue<T>(int index, T value, ICsvConverter valueConverter)
        {
            this[index] = valueConverter.ToString(value);
        }

        /// <summary>
        /// The current csv record as list of values
        /// </summary>
        /// <returns>List of values</returns>
        public List<string> ToList()
        {
            return _csvValues.ToList();
        }

        /// <summary>
        /// Tries to set the value of the current csv record at the posiiton of the specified header name.
        /// </summary>
        /// <param name="name">Name of a header</param>
        /// <param name="value">A value</param>
        /// <returns>true if position within the current csv record was found; otherwise, false.</returns>
        public bool TrySetValue(string name, string value)
        {
            var i = Headers.IndexOf(x => x == name);
            if (i != -1)
            {
                return TrySetValue(i, value);
            }
            return false;
        }

        /// <summary>
        /// Tries to set the value of the current csv record at the specified index. 
        /// </summary>
        /// <param name="index">Index within the current csv record</param>
        /// <param name="value">A value</param>
        /// <returns>true if position within the current csv record was found; otherwise, false.</returns>
        public bool TrySetValue(int index, string value)
        {
            if (Enumerable.Range(0, Headers.Count).Contains(index))
            {
                _csvValues[index] = value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries to set the value of the current csv record at the posiiton of the specified header name.
        /// </summary>
        /// <param name="name">Name of a header</param>
        /// <param name="value">A value</param>
        /// <returns>true if position within the current csv record was found; otherwise, false.</returns>
        public bool TrySetValue<T>(string name, T value)
        {
            var i = Headers.IndexOf(x => x == name);
            if (i != -1)
            {
                return TrySetValue(i, value);
            }
            return false;
        }

        /// <summary>
        /// Tries to set the value of the current csv record at the specified index. 
        /// </summary>
        /// <param name="index">Index within the current csv record</param>
        /// <param name="value">A value</param>
        /// <returns>true if position within the current csv record was found; otherwise, false.</returns>
        public bool TrySetValue<T>(int index, T value)
        {
            if (Enumerable.Range(0, Headers.Count).Contains(index))
            {
                SetValue(index, value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Writes the current csv record to the stream and clears its cotnent
        /// </summary>
        public string Write()
        {
            var line = _csvLineBuilder.Write(_csvValues);
            Array.Fill(_csvValues, null);
            return line;
        }

        /// <summary>
        /// Writes the csv headers to the stream.
        /// </summary>
        public string WriteHeaders()
        {
            int i = 0;
            foreach (var value in Headers)
            {
                SetValue(i, value);
                i++;
            }
            return Write();
        }

        /// <summary>
        /// Writes the csv headers to the stream.
        /// </summary>
        /// <param name="csvHeaders">List of csv headers</param>
        public string WriteHeaders(IEnumerable<string> csvHeaders)
        {
            Headers.Replace(csvHeaders);
            Array.Resize(ref _csvValues, csvHeaders.Count());
            return WriteHeaders();
        }

        /// <summary>
        /// Writes the csv header values to the stream.
        /// </summary>
        /// <param name="csvHeaders">List of csv headers</param>
        public string WriteHeaders(params string[] csvHeaders)
        {
            return WriteHeaders((IEnumerable<string>)csvHeaders);
        }
    }
}
