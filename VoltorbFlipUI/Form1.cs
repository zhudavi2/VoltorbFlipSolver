//
//  Form1.cs
//
//  Copyright (c) 2014 David Zhu. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VoltorbFlipUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.gridTextBoxes = new TextBox[ VoltorbFlipGridDllWrapper.GridSize, VoltorbFlipGridDllWrapper.GridSize ];
            for ( int r=0; r<VoltorbFlipGridDllWrapper.GridSize; r++ )
            {
                for ( int c=0; c<VoltorbFlipGridDllWrapper.GridSize; c++ )
                {
                    this.gridTextBoxes[ r, c ] = new TextBox();

                    this.gridTextBoxes[ r, c ].Location = new Point(
                        Form1.gridSpacing +
                        c * ( Form1.smallGridLength + Form1.gridSpacing ),

                        Form1.gridSpacing +
                        r * ( Form1.smallGridLength + Form1.gridSpacing ) );

                    this.gridTextBoxes[ r, c ].MinimumSize = new Size(
                        Form1.smallGridLength, Form1.smallGridLength );
                    this.gridTextBoxes[ r, c ].Size = new Size(
                        Form1.smallGridLength, Form1.smallGridLength );

                    this.gridTextBoxes[ r, c ].MaxLength = 2;

                    // Row clues, column clues, then grid.
                    this.gridTextBoxes[ r, c ].TabIndex =
                        2 * VoltorbFlipGridDllWrapper.GridSize + 2 * VoltorbFlipGridDllWrapper.GridSize + ( r * VoltorbFlipGridDllWrapper.GridSize + c );

                    this.gridTextBoxes[ r, c ].GotFocus += TextBox_GotFocus;

                    this.Controls.Add( this.gridTextBoxes[ r, c ] );
                }
            }

            this.rowClueTextBoxes = new Tuple<TextBox, TextBox>[ VoltorbFlipGridDllWrapper.GridSize ];
            for ( int r=0; r<VoltorbFlipGridDllWrapper.GridSize; r++ )
            {
                this.rowClueTextBoxes[ r ] = new Tuple<TextBox, TextBox>( new TextBox(), new TextBox() );
                
                // Sum
                this.rowClueTextBoxes[ r ].Item1.Location = new Point(
                    this.gridTextBoxes[ r, VoltorbFlipGridDllWrapper.GridSize-1 ].Right +
                    3 * Form1.gridSpacing,

                    this.gridTextBoxes[ r, VoltorbFlipGridDllWrapper.GridSize-1 ].Location.Y );

                this.rowClueTextBoxes[ r ].Item1.Size = new Size(
                    Form1.smallGridLength, Form1.smallGridLength );

                this.rowClueTextBoxes[ r ].Item1.MaxLength = 2;
                this.rowClueTextBoxes[ r ].Item1.TabIndex = 2 * r;
                this.rowClueTextBoxes[ r ].Item1.GotFocus += TextBox_GotFocus;
                this.Controls.Add( this.rowClueTextBoxes[ r ].Item1 );

                // Zeroes
                this.rowClueTextBoxes[ r ].Item2.Location = new Point(
                    this.rowClueTextBoxes[ r ].Item1.Right +
                    Form1.gridSpacing,

                    this.gridTextBoxes[ r, VoltorbFlipGridDllWrapper.GridSize-1 ].Location.Y );

                this.rowClueTextBoxes[ r ].Item2.Size = new Size(
                    Form1.smallGridLength, Form1.smallGridLength );

                this.rowClueTextBoxes[ r ].Item2.MaxLength = 1;
                this.rowClueTextBoxes[ r ].Item2.TabIndex = 2 * r + 1;
                this.rowClueTextBoxes[ r ].Item2.GotFocus += TextBox_GotFocus;
                this.Controls.Add( this.rowClueTextBoxes[ r ].Item2 );
            }

            this.columnClueTextBoxes = new Tuple<TextBox, TextBox>[ VoltorbFlipGridDllWrapper.GridSize ];
            for ( int c=0; c<VoltorbFlipGridDllWrapper.GridSize; c++ )
            {
                this.columnClueTextBoxes[ c ] = new Tuple<TextBox, TextBox>( new TextBox(), new TextBox() );

                // Sum
                this.columnClueTextBoxes[ c ].Item1.Location = new Point(
                    this.gridTextBoxes[ VoltorbFlipGridDllWrapper.GridSize-1, c ].Location.X,

                    this.gridTextBoxes[ VoltorbFlipGridDllWrapper.GridSize-1, c ].Bottom +
                    3 * Form1.gridSpacing );

                this.columnClueTextBoxes[ c ].Item1.Size = new Size(
                    Form1.smallGridLength, Form1.smallGridLength );

                this.columnClueTextBoxes[ c ].Item1.MaxLength = 2;
                this.columnClueTextBoxes[ c ].Item1.TabIndex = 2 * VoltorbFlipGridDllWrapper.GridSize + 2 * c;
                this.columnClueTextBoxes[ c ].Item1.GotFocus += TextBox_GotFocus;
                this.Controls.Add( this.columnClueTextBoxes[ c ].Item1 );

                // Zeroes
                this.columnClueTextBoxes[ c ].Item2.Location = new Point(
                    this.columnClueTextBoxes[ c ].Item1.Location.X,

                    this.columnClueTextBoxes[ c ].Item1.Bottom +
                    Form1.gridSpacing );

                this.columnClueTextBoxes[ c ].Item2.Size = new Size(
                    Form1.smallGridLength, Form1.smallGridLength );

                this.columnClueTextBoxes[ c ].Item2.MaxLength = 1;
                this.columnClueTextBoxes[ c ].Item2.TabIndex = 2 * VoltorbFlipGridDllWrapper.GridSize + 2 * c + 1;
                this.columnClueTextBoxes[ c ].Item2.GotFocus += TextBox_GotFocus;
                this.Controls.Add( this.columnClueTextBoxes[ c ].Item2 );
            }

            this.solveButton.Location = new Point(
                this.solveButton.Location.X,

                this.columnClueTextBoxes.First().Item2.Bottom +
                2 * Form1.gridSpacing );
            this.solveButton.TabIndex = this.gridTextBoxes[ VoltorbFlipGridDllWrapper.GridSize-1, VoltorbFlipGridDllWrapper.GridSize-1 ].TabIndex + 1;

            this.clearGridButton.Location = new Point(
                this.clearGridButton.Location.X,
                this.solveButton.Location.Y );
            this.clearGridButton.TabIndex = this.solveButton.TabIndex + 1;

            this.clearAllButton.Location = new Point(
                this.clearAllButton.Location.X,
                this.solveButton.Location.Y );
            this.clearAllButton.TabIndex = this.clearGridButton.TabIndex + 1;

            // For percentages.
            this.chanceLabels = new Label[ VoltorbFlipGridDllWrapper.GridSize, VoltorbFlipGridDllWrapper.GridSize ];
            for ( int r=0; r<VoltorbFlipGridDllWrapper.GridSize; r++ )
            {
                for ( int c=0; c<VoltorbFlipGridDllWrapper.GridSize; c++ )
                {
                    this.chanceLabels[ r, c ] = new Label();

                    this.chanceLabels[ r, c ].Location = new Point(
                        this.rowClueTextBoxes.Last().Item2.Right +
                        4 * Form1.gridSpacing +
                        c * ( Form1.bigGridLength + Form1.gridSpacing ),

                        Form1.gridSpacing +
                        r * ( Form1.bigGridLength + Form1.gridSpacing ) );

                    this.chanceLabels[ r, c ].Size = new Size(
                        Form1.bigGridLength, Form1.bigGridLength );
                    this.chanceLabels[ r, c ].BorderStyle = BorderStyle.FixedSingle;

                    this.Controls.Add( this.chanceLabels[ r, c ] );
                }
            }
        }

        void TextBox_GotFocus( object sender, EventArgs e )
        {
            if ( sender is TextBox )
            {
                ( sender as TextBox ).SelectAll();
            }
        }

        private void solveButton_Click(object sender, EventArgs e)
        {
            //VoltorbFlipGridDllWrapper.solveGridWithIntValues
            var rowClues = new Tuple<int, int>[ VoltorbFlipGridDllWrapper.GridSize ];
            for ( int r=0; r<VoltorbFlipGridDllWrapper.GridSize; r++ )
            {
                rowClues[ r ] = new Tuple<int, int>(
                    int.Parse( this.rowClueTextBoxes[r].Item1.Text ),
                    int.Parse( this.rowClueTextBoxes[r].Item2.Text ) );
            }

            var columnClues = new Tuple<int, int>[ VoltorbFlipGridDllWrapper.GridSize ];
            for ( int c=0; c<VoltorbFlipGridDllWrapper.GridSize; c++ )
            {
                columnClues[ c ] = new Tuple<int, int>(
                    int.Parse( this.columnClueTextBoxes[ c ].Item1.Text ),
                    int.Parse( this.columnClueTextBoxes[ c ].Item2.Text ) );
            }

            var gridIntValues = new int[ VoltorbFlipGridDllWrapper.GridSize, VoltorbFlipGridDllWrapper.GridSize ];
            for ( int r=0; r<VoltorbFlipGridDllWrapper.GridSize; r++ )
            {
                for (int c=0; c<VoltorbFlipGridDllWrapper.GridSize; c++ )
                {
                    int gridIntValue = 0;
                    var tryParseSuccess = int.TryParse( this.gridTextBoxes[ r, c ].Text, out gridIntValue );

                    // Invalid value means unknown.
                    if ( !tryParseSuccess )
                    {
                        gridIntValue = VoltorbFlipGridDllWrapper.MaximumIntValueForZeroOrOne + 1;
                    }

                    gridIntValues[ r, c ] = gridIntValue;
                }
            }

            var possibilities = VoltorbFlipGridDllWrapper.solveGridWithIntValues(rowClues, columnClues, gridIntValues);
            int numberOfSolutions = 0;
            foreach ( var possibilitiesDic in possibilities[0,0] )
            {
                numberOfSolutions += possibilitiesDic.Value;
            }

            Console.WriteLine( "Number of solutions: " + numberOfSolutions );

            if ( numberOfSolutions > 0 )
            {
                var probabilities = new float[ VoltorbFlipGridDllWrapper.GridSize, VoltorbFlipGridDllWrapper.GridSize ][];
                for ( int r=0; r<VoltorbFlipGridDllWrapper.GridSize; r++ )
                {
                    for ( int c=0; c<VoltorbFlipGridDllWrapper.GridSize; c++ )
                    {
                        // See if the number of solutions is consistent.
                        {
                            int testNumberOfSolutions = 0;
                            foreach ( var possibilitiesDic in possibilities[ r, c ] )
                            {
                                testNumberOfSolutions += possibilitiesDic.Value;
                            }
                            System.Diagnostics.Debug.Assert( testNumberOfSolutions == numberOfSolutions, "Number of solutions is not consistent!" );
                        }

                        probabilities[ r, c ] = new float[ VoltorbFlipGridDllWrapper.NumberOfValues - 1 ];
                        probabilities[ r, c ][ 0 ] = ( float )possibilities[ r, c ][ VoltorbFlipGridDllWrapper.VoltorbFlipValueManaged.VFVMZero ] / ( float )numberOfSolutions;
                        probabilities[ r, c ][ 1 ] = ( float )possibilities[ r, c ][ VoltorbFlipGridDllWrapper.VoltorbFlipValueManaged.VFVMOne ] / ( float )numberOfSolutions;
                        probabilities[ r, c ][ 2 ] =
                            ( float )( possibilities[ r, c ][ VoltorbFlipGridDllWrapper.VoltorbFlipValueManaged.VFVMTwo ] + possibilities[ r, c ][ VoltorbFlipGridDllWrapper.VoltorbFlipValueManaged.VFVMThree ] ) / ( float )numberOfSolutions;

                        float nonZeroChance = 1.0f - probabilities[ r, c ][ 0 ];
                        int nonZeroPercentage = ( int )( nonZeroChance * 100 );

                        int twoOrThreePercentage = ( int )( ( probabilities[ r, c ][ 2 ] ) * 100 );

                        this.chanceLabels[ r, c ].Text =
                            nonZeroPercentage.ToString() + '%' +
                            System.Environment.NewLine +
                            twoOrThreePercentage + '%';

                        // TextBox colours
                        var chanceLabelBackgroundColour = this.ZeroColour;
                        int gridIntValue = 0;
                        if ( int.TryParse( this.gridTextBoxes[ r, c ].Text, out gridIntValue ) )
                        {
                            // Already filled in.
                            switch ( gridIntValue )
                            {
                                // Filled-in 0: Black
                                case ( int )VoltorbFlipGridDllWrapper.VoltorbFlipValueManaged.VFVMZero:
                                    chanceLabelBackgroundColour = this.ZeroColour;
                                    break;

                                // Filled-in 1, 2, or 3: Blue
                                case ( int )VoltorbFlipGridDllWrapper.VoltorbFlipValueManaged.VFVMOne:
                                case ( int )VoltorbFlipGridDllWrapper.VoltorbFlipValueManaged.VFVMTwo:
                                case ( int )VoltorbFlipGridDllWrapper.VoltorbFlipValueManaged.VFVMThree:
                                    chanceLabelBackgroundColour = this.FilledInColour;
                                    break;

                                // Filled-in, but 0 or 1
                                default:
                                    if ( possibilities[ r, c ][ VoltorbFlipGridDllWrapper.VoltorbFlipValueManaged.VFVMZero ] == numberOfSolutions )
                                    {
                                        // Definitely 0
                                        chanceLabelBackgroundColour = this.ZeroColour;
                                    }
                                    else if ( possibilities[ r, c ][ VoltorbFlipGridDllWrapper.VoltorbFlipValueManaged.VFVMOne ] == numberOfSolutions )
                                    {
                                        // Definitely 1, or safe.
                                        chanceLabelBackgroundColour = Color.Green;
                                    }
                                    else
                                    {
                                        // 0 or 1 and we're not sure which.
                                        chanceLabelBackgroundColour = this.ZeroOrOneColour;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            // Not filled-in, unknown.
                            if ( ( 0 < possibilities[ r, c ][ VoltorbFlipGridDllWrapper.VoltorbFlipValueManaged.VFVMZero ] ) && ( possibilities[ r, c ][ VoltorbFlipGridDllWrapper.VoltorbFlipValueManaged.VFVMZero ] < numberOfSolutions ) && ( twoOrThreePercentage == 0 ) )
                            {
                                // 0 or 1, and we're not sure which.
                                chanceLabelBackgroundColour = this.ZeroOrOneColour;
                            }
                            else
                            {
                                // Gradient otherwise.
                                chanceLabelBackgroundColour = this.getColorFromNonZeroChance( nonZeroChance );
                            }
                        }

                        this.chanceLabels[ r, c ].BackColor = chanceLabelBackgroundColour;
                    }
                }
            }
        }

        private Color getColorFromNonZeroChance( float nonZeroChance )
        {
            const float chanceBoundary1 = 0.5f; 
            const float chanceBoundary2 = 0.75f;
            const int maximumColourAmount = 255;

            int boxRed = 0;
            int boxGreen = 0;
            if ( nonZeroChance <= chanceBoundary1 )
            {
                boxRed = ( int )( 2 * nonZeroChance * maximumColourAmount );
                boxGreen = 0;
            }
            else if ( ( chanceBoundary1 < nonZeroChance ) && ( nonZeroChance <= chanceBoundary2 ) )
            {
                boxRed = maximumColourAmount;
                boxGreen = ( int )( 4 * ( nonZeroChance - chanceBoundary1 ) * maximumColourAmount );
            }
            else
            {
                boxRed = ( int )( ( -4 * nonZeroChance + 4 ) * maximumColourAmount );
                boxGreen = maximumColourAmount;
            }

            return Color.FromArgb( boxRed, boxGreen, 0 );
        }

        private void clearGridButton_Click( object sender, EventArgs e )
        {
            foreach ( var gridBox in this.gridTextBoxes )
            {
                gridBox.Clear();
            }
        }

        private void clearAllButton_Click( object sender, EventArgs e )
        {
            this.clearGridButton_Click( sender, e );

            foreach ( var rowClueBox in this.rowClueTextBoxes )
            {
                rowClueBox.Item1.Clear();
                rowClueBox.Item2.Clear();
            }

            foreach ( var columnClueBox in this.columnClueTextBoxes )
            {
                columnClueBox.Item1.Clear();
                columnClueBox.Item2.Clear();
            }
        }

        private const int bigGridLength = 40;
        private const int smallGridLength = bigGridLength;
        private const int gridSpacing = 5;

        private readonly Color FilledInColour = Color.Blue;
        private readonly Color ZeroColour = Color.Black;
        private readonly Color ZeroOrOneColour = Color.Purple;

        private TextBox[ , ] gridTextBoxes;
        private Tuple< TextBox, TextBox >[] rowClueTextBoxes;
        private Tuple< TextBox, TextBox >[] columnClueTextBoxes;

        private Label[ , ] chanceLabels;
    }
}
