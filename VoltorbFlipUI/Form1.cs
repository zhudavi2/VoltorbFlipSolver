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

            this.gridSize = VoltorbFlipGridDllWrapper.getGridSize();
            this.numberOfPossibleValues = VoltorbFlipGridDllWrapper.getNumberOfPossibleValues();
            this.zeroOrOneValue = VoltorbFlipGridDllWrapper.getCouldBeZeroOrOneValue();
            this.unknownValue = VoltorbFlipGridDllWrapper.getUnknownValue();

            this.gridTextBoxes = new TextBox[ this.gridSize, this.gridSize ];
            for ( int r=0; r<this.gridSize; r++ )
            {
                for ( int c=0; c<this.gridSize; c++ )
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
                        2 * this.gridSize + 2 * this.gridSize + ( r * this.gridSize + c );

                    this.Controls.Add( this.gridTextBoxes[ r, c ] );
                }
            }

            this.rowClueTextBoxes = new Tuple<TextBox, TextBox>[ this.gridSize ];
            for ( int r=0; r<this.gridSize; r++ )
            {
                this.rowClueTextBoxes[ r ] = new Tuple<TextBox, TextBox>( new TextBox(), new TextBox() );
                
                // Sum
                this.rowClueTextBoxes[ r ].Item1.Location = new Point(
                    this.gridTextBoxes[ r, this.gridSize-1 ].Right +
                    3 * Form1.gridSpacing,

                    this.gridTextBoxes[ r, this.gridSize-1 ].Location.Y );

                this.rowClueTextBoxes[ r ].Item1.Size = new Size(
                    Form1.smallGridLength, Form1.smallGridLength );

                this.rowClueTextBoxes[ r ].Item1.MaxLength = 2;
                this.rowClueTextBoxes[ r ].Item1.TabIndex = 2 * r;
                this.Controls.Add( this.rowClueTextBoxes[ r ].Item1 );

                // Zeroes
                this.rowClueTextBoxes[ r ].Item2.Location = new Point(
                    this.rowClueTextBoxes[ r ].Item1.Right +
                    Form1.gridSpacing,

                    this.gridTextBoxes[ r, this.gridSize-1 ].Location.Y );

                this.rowClueTextBoxes[ r ].Item2.Size = new Size(
                    Form1.smallGridLength, Form1.smallGridLength );

                this.rowClueTextBoxes[ r ].Item2.MaxLength = 1;
                this.rowClueTextBoxes[ r ].Item2.TabIndex = 2 * r + 1;
                this.Controls.Add( this.rowClueTextBoxes[ r ].Item2 );
            }

            this.columnClueTextBoxes = new Tuple<TextBox, TextBox>[ this.gridSize ];
            for ( int c=0; c<this.gridSize; c++ )
            {
                this.columnClueTextBoxes[ c ] = new Tuple<TextBox, TextBox>( new TextBox(), new TextBox() );

                // Sum
                this.columnClueTextBoxes[ c ].Item1.Location = new Point(
                    this.gridTextBoxes[ this.gridSize-1, c ].Location.X,

                    this.gridTextBoxes[ this.gridSize-1, c ].Bottom +
                    3 * Form1.gridSpacing );

                this.columnClueTextBoxes[ c ].Item1.Size = new Size(
                    Form1.smallGridLength, Form1.smallGridLength );

                this.columnClueTextBoxes[ c ].Item1.MaxLength = 2;
                this.columnClueTextBoxes[ c ].Item1.TabIndex = 2 * this.gridSize + 2 * c;
                this.Controls.Add( this.columnClueTextBoxes[ c ].Item1 );

                // Zeroes
                this.columnClueTextBoxes[ c ].Item2.Location = new Point(
                    this.columnClueTextBoxes[ c ].Item1.Location.X,

                    this.columnClueTextBoxes[ c ].Item1.Bottom +
                    Form1.gridSpacing );

                this.columnClueTextBoxes[ c ].Item2.Size = new Size(
                    Form1.smallGridLength, Form1.smallGridLength );

                this.columnClueTextBoxes[ c ].Item2.MaxLength = 1;
                this.columnClueTextBoxes[ c ].Item2.TabIndex = 2 * this.gridSize + 2 * c + 1;
                this.Controls.Add( this.columnClueTextBoxes[ c ].Item2 );
            }

            this.solveButton.Location = new Point(
                this.columnClueTextBoxes.First().Item2.Location.X,

                this.columnClueTextBoxes.First().Item2.Bottom +
                2 * Form1.gridSpacing );
            this.solveButton.TabIndex = this.gridTextBoxes[ this.gridSize-1, this.gridSize-1 ].TabIndex + 1;

            // For percentages.
            this.chanceTextBoxes = new TextBox[ this.gridSize, this.gridSize ];
            for ( int r=0; r<this.gridSize; r++ )
            {
                for ( int c=0; c<this.gridSize; c++ )
                {
                    this.chanceTextBoxes[ r, c ] = new TextBox();

                    this.chanceTextBoxes[ r, c ].Location = new Point(
                        this.rowClueTextBoxes.Last().Item2.Right +
                        4 * Form1.gridSpacing +
                        c * ( Form1.bigGridLength + Form1.gridSpacing ),

                        Form1.gridSpacing +
                        r * ( Form1.bigGridLength + Form1.gridSpacing ) );

                    this.chanceTextBoxes[ r, c ].Size = new Size(
                        Form1.bigGridLength, Form1.bigGridLength );

                    this.chanceTextBoxes[ r, c ].ReadOnly = true;
                    this.chanceTextBoxes[ r, c ].Multiline = true;

                    this.Controls.Add( this.chanceTextBoxes[ r, c ] );
                }
            }

            this.Width =
                this.chanceTextBoxes[ this.gridSize-1, this.gridSize-1 ].Right +
                Form1.gridSpacing;
            this.Height = this.solveButton.Bottom + Form1.gridSpacing;
        }

        private void solveButton_Click(object sender, EventArgs e)
        {
            foreach ( var row in this.chanceTextBoxes )
            {
                row.BackColor = Color.White;
            }

            var rowClues = new Tuple<int, int>[ this.gridSize ];
            for ( int r=0; r<this.gridSize; r++ )
            {
                rowClues[ r ] = new Tuple<int, int>(
                    int.Parse( this.rowClueTextBoxes[r].Item1.Text ),
                    int.Parse( this.rowClueTextBoxes[r].Item2.Text ) );
            }

            var columnClues = new Tuple<int, int>[ this.gridSize ];
            for ( int c=0; c<this.gridSize; c++ )
            {
                columnClues[ c ] = new Tuple<int, int>(
                    int.Parse( this.columnClueTextBoxes[ c ].Item1.Text ),
                    int.Parse( this.columnClueTextBoxes[ c ].Item2.Text ) );
            }

            var gridValues = new int[ this.gridSize, this.gridSize ];
            for ( int r=0; r<this.gridSize; r++ )
            {
                for (int c=0; c<this.gridSize; c++ )
                {
                    int gridValue = 0;
                    var tryParseSuccess = int.TryParse( this.gridTextBoxes[ r, c ].Text, out gridValue );
                    if ( ( !tryParseSuccess ) ||
                        ( ( gridValue >= this.numberOfPossibleValues ) &&
                          ( gridValue != this.zeroOrOneValue ) ) )
                    {
                        gridValue = this.unknownValue;
                    }
                    else
                    {
                        if (gridValue == this.zeroOrOneValue )
                        {
                            this.chanceTextBoxes[ r, c ].BackColor = Color.Purple;
                        }
                        else if ( gridValue == 0 )
                        {
                            this.chanceTextBoxes[ r, c ].BackColor = Color.Black;
                        }
                        else
                        {
                            this.chanceTextBoxes[ r, c ].BackColor = Color.Blue;
                        }
                    }

                    gridValues[ r, c ] = gridValue;
                }
            }

            var possibilities = VoltorbFlipGridDllWrapper.solve(rowClues, columnClues, gridValues);
            int numberOfSolutions = 0;
            foreach ( int count in possibilities[0,0] )
            {
                numberOfSolutions += count;
            }

            Console.WriteLine( "Number of solutions: " + numberOfSolutions );

            if ( numberOfSolutions > 0 )
            {
                var probabilities = new float[ this.gridSize, this.gridSize ][];
                for ( int r=0; r<this.gridSize; r++ )
                {
                    for (int c=0; c<this.gridSize; c++)
                    {
                        // See if the number of solutions is consistent.
                        {
                            int testNumberOfSolutions = 0;
                            foreach ( int count in possibilities[ r, c ] )
                            {
                                testNumberOfSolutions += count;
                            }
                            System.Diagnostics.Debug.Assert(testNumberOfSolutions == numberOfSolutions, "Number of solutions is not consistent!");
                        }

                        probabilities[ r, c ] = new float[ this.numberOfPossibleValues - 1 ];
                        probabilities[ r, c ][ 0 ] = ( float )possibilities[ r, c ][ 0 ] / ( float )numberOfSolutions;
                        probabilities[ r, c ][ 1 ] = ( float )possibilities[ r, c ][ 1 ] / ( float )numberOfSolutions;
                        probabilities[ r, c ][ 2 ] =
                            ( float )( possibilities[ r, c ][ 2 ] + possibilities[ r, c ][ 3 ]) / ( float )numberOfSolutions;

                        float nonZeroChance = (float)1 - probabilities[ r, c ][ 0 ];
                        int nonZeroPercentage = ( int )( nonZeroChance * 100 );

                        int twoOrThreePercentage = ( int )( ( probabilities[ r, c ][ 2 ] ) * 100 );

                        this.chanceTextBoxes[r,c].Text =
                            nonZeroPercentage.ToString() + '%' +
                            System.Environment.NewLine +
                            twoOrThreePercentage + '%';

                        // TextBox colours
                        if ( this.chanceTextBoxes[ r, c ].BackColor == Color.White )
                        {
                            if ( probabilities[ r, c ][ 2 ] != 0 )
                            {
                                this.chanceTextBoxes[ r, c ].BackColor = this.getColorFromNonZeroChance( nonZeroChance );
                            }
                            else 
                            {
                                if ( nonZeroPercentage == 0 )
                                {
                                    this.chanceTextBoxes[ r, c ].BackColor = Color.Black;
                                }
                                else if (nonZeroPercentage == 100)
                                {
                                    this.chanceTextBoxes[ r, c ].BackColor = this.getColorFromNonZeroChance( nonZeroChance );
                                }
                                else
                                {
                                    this.chanceTextBoxes[ r, c ].BackColor = Color.Purple;
                                }
                            }
                        }
                        else if (this.chanceTextBoxes[r,c].BackColor == Color.Purple)
                        {
                            if ( nonZeroPercentage == 0 )
                            {
                                this.chanceTextBoxes[ r, c ].BackColor = Color.Black;
                            }
                            else if ( nonZeroPercentage == 100 )
                            {
                                this.chanceTextBoxes[ r, c ].BackColor = this.getColorFromNonZeroChance(1);
                            }
                        }
                    }
                }
            }
        }

        private Color getColorFromNonZeroChance( float nonZeroChance )
        {
            int boxRed = 0;
            int boxGreen = 0;
            if ( nonZeroChance <= 0.5 )
            {
                boxRed = ( int )( 2 * nonZeroChance * 255 );
                boxGreen = 0;
            }
            else if ( ( 0.5 < nonZeroChance ) && ( nonZeroChance <= 0.75 ) )
            {
                boxRed = 255;
                boxGreen = ( int )( 4 * ( nonZeroChance - 0.5 ) * 255 );
            }
            else
            {
                boxRed = ( int )( ( -4 * nonZeroChance + 4 ) * 255 );
                boxGreen = ( int )( ( -2 * nonZeroChance + 2.5 ) * 255 );
            }

            return Color.FromArgb( boxRed, boxGreen, 0 );
        }

        private const int bigGridLength = 40;
        private const int smallGridLength = bigGridLength;
        private const int gridSpacing = 5;

        private readonly int gridSize;
        private readonly int numberOfPossibleValues;
        private readonly int zeroOrOneValue;
        private readonly int unknownValue;

        private TextBox[ , ] gridTextBoxes;
        private Tuple< TextBox, TextBox >[] rowClueTextBoxes;
        private Tuple<TextBox, TextBox>[] columnClueTextBoxes;

        private TextBox[ , ] chanceTextBoxes;
    }
}
