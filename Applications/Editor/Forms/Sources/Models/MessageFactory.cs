﻿/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
using Cube.FileSystem;
using Cube.Xui;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MessageFactory
    ///
    /// <summary>
    /// Provides functionality to create messsage objects.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class MessageFactory
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// CreateSource
        ///
        /// <summary>
        /// Creates a message to show the OpenFileDialog.
        /// </summary>
        ///
        /// <param name="callback">
        /// Callback action when terminating the user operation.
        /// </param>
        ///
        /// <returns>OpenFileMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenFileMessage CreateSource(OpenFileCallback callback) =>
            new OpenFileMessage(callback)
            {
                Title           = Properties.Resources.TitleOpen,
                CheckPathExists = true,
                Multiselect     = false,
                Filter          = new []
                {
                    new DisplayFilter(Properties.Resources.FilterPdf, true, ".pdf"),
                    new DisplayFilter(Properties.Resources.FilterAll, true, ".*"),
                }.GetFilter(),
            };

        #endregion
    }
}
