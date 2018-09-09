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
    /// MainBindable
    ///
    /// <summary>
    /// Provides values for binding to the MainWindow.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class MainBindable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainBindable
        ///
        /// <summary>
        /// Initializes a new instance of the <c>MainBindable</c> class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="images">Image collection.</param>
        /// <param name="settings">Settings object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainBindable(ImageCollection images, SettingsFolder settings)
        {
            Images    = images;
            _settings = settings;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Images
        ///
        /// <summary>
        /// Gets an image collection of PDF documents.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageCollection Images { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Selection
        ///
        /// <summary>
        /// Gets the selection of thumbnails.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageSelection Selection => Images.Selection;

        /* ----------------------------------------------------------------- */
        ///
        /// Preferences
        ///
        /// <summary>
        /// Gets the preferences for thumbnails.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImagePreferences Preferences => Images.Preferences;

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// Gets an application settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Settings Settings => _settings.Value;

        /* ----------------------------------------------------------------- */
        ///
        /// History
        ///
        /// <summary>
        /// Gets a history to execute the undo and redo actions.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public History History { get; } = new History();

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets a file path of the PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<Information> Source { get; } = new Bindable<Information>();

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the number of pages in the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<int> Count { get; } = new Bindable<int>(0);

        /* ----------------------------------------------------------------- */
        ///
        /// IsOpen
        ///
        /// <summary>
        /// Gets a value that determines whether a PDF document is open.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<bool> IsOpen { get; } = new Bindable<bool>(false);

        /* ----------------------------------------------------------------- */
        ///
        /// IsBusy
        ///
        /// <summary>
        /// Gets a value that determines whether models are busy.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<bool> IsBusy { get; } = new Bindable<bool>(false);

        /* ----------------------------------------------------------------- */
        ///
        /// Message
        ///
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<string> Message { get; } = new Bindable<string>(string.Empty);

        #endregion

        #region Fields
        private SettingsFolder _settings;
        #endregion
    }
}