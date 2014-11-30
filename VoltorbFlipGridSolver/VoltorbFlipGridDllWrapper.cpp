//
//  VoltorbFlipGridDllWrapper.cpp
//
//  Copyright (c) 2014 David Zhu. All rights reserved.
//

#include "VoltorbFlipGridDllWrapper.h"

array< System::Collections::Generic::IDictionary< VoltorbFlipGridDllWrapper::VoltorbFlipValueManaged, int >^, 2 >^ VoltorbFlipGridDllWrapper::solveGridWithIntValues( array< System::Tuple< int, int >^ >^ rowClues, array< System::Tuple< int, int >^ >^ columnClues, array< int, 2 >^ gridIntValues )
{
    auto nativeRowClues = VoltorbFlipGridDllWrapper::convertCluesToNative( rowClues );
    auto nativeColumnClues = VoltorbFlipGridDllWrapper::convertCluesToNative( columnClues );

    VoltorbFlipGridSolver gridSolver( nativeRowClues, nativeColumnClues );

    for ( int r=0; r<GridSize; r++ )
    {
        for ( int c=0; c<GridSize; c++ )
        {
            // Can't directly use gridIntValues[ r, c ] as a parameter to std::map< int, VoltorbFlipValue >::count()
            int gridIntValue = gridIntValues[ r, c ];

            auto nativeGridValue = VoltorbFlipGrid::VoltorbFlipValue::VFVUnknown;
            if ( VoltorbFlipGridSolver::IntToVoltorbFlipValue.count( gridIntValue ) )
            {
                nativeGridValue = VoltorbFlipGridSolver::IntToVoltorbFlipValue.at( gridIntValue );
            }
            else if ( gridIntValue <= MaximumIntValueForZeroOrOne )
            {
                nativeGridValue = VoltorbFlipGrid::VoltorbFlipValue::VFVZeroOrOne;
            }

            gridSolver.setEntryValue( r, c, nativeGridValue );
        }
    }

    gridSolver.solve();

    auto countDictionary = gcnew array< System::Collections::Generic::IDictionary< VoltorbFlipValueManaged, int >^, 2 >( GridSize, GridSize );
    for ( int r=0; r<GridSize; r++ )
    {
        for ( int c=0; c<GridSize; c++ )
        {
            countDictionary[ r, c ] = gcnew System::Collections::Generic::Dictionary< VoltorbFlipValueManaged, int >;

            countDictionary[ r, c ][ VoltorbFlipValueManaged::VFVMZero ] =
                gridSolver.getValueCountOfEntry( r, c, VoltorbFlipGrid::VoltorbFlipValue::VFVZero );
            countDictionary[ r, c ][ VoltorbFlipValueManaged::VFVMOne ] =
                gridSolver.getValueCountOfEntry( r, c, VoltorbFlipGrid::VoltorbFlipValue::VFVOne );
            countDictionary[ r, c ][ VoltorbFlipValueManaged::VFVMTwo ] =
                gridSolver.getValueCountOfEntry( r, c, VoltorbFlipGrid::VoltorbFlipValue::VFVTwo );
            countDictionary[ r, c ][ VoltorbFlipValueManaged::VFVMThree ] =
                gridSolver.getValueCountOfEntry( r, c, VoltorbFlipGrid::VoltorbFlipValue::VFVThree );
        }
    }

    return countDictionary;
}

VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipGridSolver::VoltorbFlipClue > VoltorbFlipGridDllWrapper::convertCluesToNative( array< System::Tuple< int, int >^ >^ lineClues )
{
    VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipGridSolver::VoltorbFlipClue > nativeLineClues;

    for ( int i=0; i<GridSize; i++ )
    {
        nativeLineClues[ i ].first = lineClues[ i ]->Item1;
        nativeLineClues[ i ].second = lineClues[ i ]->Item2;
    }

    return nativeLineClues;
}
