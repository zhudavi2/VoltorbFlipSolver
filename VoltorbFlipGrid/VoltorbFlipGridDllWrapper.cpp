//
//  VoltorbFlipGridDllWrapper.cpp
//
//  Copyright (c) 2014 David Zhu. All rights reserved.
//

#include "VoltorbFlipGridDllWrapper.h"

array< array< int >^, 2 >^ VoltorbFlipGridDllWrapper::solve( array< System::Tuple< int, int >^ >^ rowClues, array< System::Tuple< int, int >^ >^ columnClues, array< int, 2 >^ gridValues )
{
    auto nativeRowClues = VoltorbFlipGridDllWrapper::convertCluesToNative( rowClues );
    auto nativeColumnClues = VoltorbFlipGridDllWrapper::convertCluesToNative( columnClues );

    VoltorbFlipGrid grid( nativeRowClues, nativeColumnClues );
    for ( int r=0; r<GridSize; r++ )
    {
        for ( int c=0; c<GridSize; c++ )
        {
            grid( r, c ) = gridValues[ r, c ];
        }
    }

    auto nativeSolutions = grid.solve();

    VoltorbFlipProbabilities nativeProbabilities( nativeSolutions );

    auto probabilities = gcnew array< array< int >^, 2 >( GridSize, GridSize );
    for ( int r=0; r<GridSize; r++ )
    {
        for ( int c=0; c<GridSize; c++ )
        {
            probabilities[ r, c ] = gcnew array< int >( NumberOfPossibleValues );
            for ( int i=0; i<NumberOfPossibleValues; i++ )
            {
                probabilities[ r, c ][ i ] = nativeProbabilities.getCountOfCell( r, c, i );
            }
        }
    }

    return probabilities;
}

VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipGrid::VoltorbFlipClue > VoltorbFlipGridDllWrapper::convertCluesToNative( array< System::Tuple< int, int >^ >^ lineClues )
{
    VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipGrid::VoltorbFlipClue > nativeLineClues;

    for ( int i=0; i<GridSize; i++ )
    {
        nativeLineClues[ i ].first = lineClues[ i ]->Item1;
        nativeLineClues[ i ].second = lineClues[ i ]->Item2;
    }

    return nativeLineClues;
}
