﻿/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using Cube.FileSystem;
using Cube.Pdf.Pdfium.PdfiumApi;
using System;
using System.Runtime.InteropServices;

namespace Cube.Pdf.Pdfium
{
    /* --------------------------------------------------------------------- */
    ///
    /// PdfReader
    ///
    /// <summary>
    /// PDFium の API をラップした Reader クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal sealed class PdfReader : PdfLibrary
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// 対象となる PDF ファイルのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private PdfReader(string src, IO io)
        {
            Source  = src;
            IO      = io;

            _stream = IO.OpenRead(src);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// 対象となる PDF ファイルのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// I/O オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// PageCount
        ///
        /// <summary>
        /// PDF のページ数を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int PageCount => NativeMethods.FPDF_GetPageCount(_document);

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// PdfReader オブジェクトを生成します。
        /// </summary>
        ///
        /// <param name="src">PDF ファイルのパス</param>
        /// <param name="query">パスワード用オブジェクト</param>
        /// <param name="io">I/O オブジェクト</param>
        /// <param name="password">入力されたパスワード</param>
        ///
        /// <returns>PdfReader</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfReader Create(string src, IQuery<string> query, IO io, out string password)
        {
            var dest = new PdfReader(src, io);
            password = string.Empty;

            while (true)
            {
                try
                {
                    dest.Load(password);
                    return dest;
                }
                catch (LoadException err)
                {
                    if (err.Status != LoadStatus.PasswordError) throw;
                    var e = QueryEventArgs.Create(src);
                    query.Request(e);
                    if (!e.Cancel) password = e.Result;
                    else throw new OperationCanceledException();
                }
            }
        }

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// リソースを開放します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (_document != IntPtr.Zero) NativeMethods.FPDF_CloseDocument(_document);
                if (disposing) _stream.Dispose();
            }
            finally { base.Dispose(disposing); }
        }

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Load
        ///
        /// <summary>
        /// PDF を読み込みます。
        /// </summary>
        ///
        /// <param name="password">パスワード</param>
        ///
        /* ----------------------------------------------------------------- */
        private void Load(string password)
        {
            _document = NativeMethods.FPDF_LoadCustomDocument(
                new FileAccess
                {
                    Length    = (uint)_stream.Length,
                    GetBlock  = Marshal.GetFunctionPointerForDelegate(new ReadDelegate(Read)),
                    Parameter = IntPtr.Zero,
                },
                password
            );

            if (_document == IntPtr.Zero) throw new LoadException(NativeMethods.FPDF_GetLastError());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Read
        ///
        /// <summary>
        /// ストリームからデータを読み込みます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private int Read(IntPtr param, uint pos, IntPtr buffer, uint size)
        {
            var managed = new byte[size];

            _stream.Position = pos;
            if (_stream.Read(managed, 0, (int)size) != size) return 0;

            Marshal.Copy(managed, 0, buffer, (int)size);
            return 1;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ReadDelegate
        ///
        /// <summary>
        /// ストリームからデータを読み込むメソッドを表します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ReadDelegate(IntPtr param, uint pos, IntPtr buffer, uint size);

        #endregion

        #region Fields
        private readonly System.IO.Stream _stream;
        private IntPtr _document;
        #endregion
    }
}
