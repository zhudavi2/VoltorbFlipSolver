//
//  VoltorbFlipGridSolver.cpp
//
//  Copyright (c) 2014 David Zhu. All rights reserved.
//

#include "VoltorbFlipGridSolver.h"
#include <cassert>

const int VoltorbFlipGridSolver::NumberOfValues = 4;

const std::map< int, VoltorbFlipGrid::VoltorbFlipValue > VoltorbFlipGridSolver::IntToVoltorbFlipValue =
{
    { 0, VoltorbFlipGrid::VoltorbFlipValue::VFVZero },
    { 1, VoltorbFlipGrid::VoltorbFlipValue::VFVOne },
    { 2, VoltorbFlipGrid::VoltorbFlipValue::VFVTwo },
    { 3, VoltorbFlipGrid::VoltorbFlipValue::VFVThree },
};

const int VoltorbFlipGridSolver::UnsolvedGridValueCount = -1;

VoltorbFlipGridSolver::VoltorbFlipGridSolver( const VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipClue >& rowClues, const VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipClue >& columnClues ) :
m_rowClues( rowClues ),
m_columnClues( columnClues ),
m_solved( false ),
m_numberOfSolutions( 0 )
{
    // Generate the reference clues.
    bool maximumLineReached = false;
    VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipGrid::VoltorbFlipValue > testReferenceLine;
    testReferenceLine.fill( VoltorbFlipGrid::VoltorbFlipValue::VFVZero );

    while ( !maximumLineReached )
    {
        auto clue = getClueFromLine( testReferenceLine );
        m_referenceClues[ clue ].push_back( testReferenceLine );

        for ( int i=0; i<VoltorbFlipGridSize; i++ )
        {
            if ( testReferenceLine[ i ] < NumberOfValues - 1 )
            {
                testReferenceLine[ i ] = IntToVoltorbFlipValue.at( testReferenceLine[ i ] + 1 );
                break;
            }
            else
            {
                if ( i < VoltorbFlipGridSize - 1 )
                {
                    testReferenceLine[ i ] =
                        VoltorbFlipGrid::VoltorbFlipValue::VFVZero;
                }
                else
                {
                    maximumLineReached = true;
                }
            }
        }
    };

    // Initialize the grid of value counts, or possibilities.
    for ( auto& countRow : m_valueCountGrid )
    {
        for ( auto& countMap : countRow )
        {
            countMap[ VoltorbFlipGrid::VoltorbFlipValue::VFVZero ] = 0;
            countMap[ VoltorbFlipGrid::VoltorbFlipValue::VFVOne ] = 0;
            countMap[ VoltorbFlipGrid::VoltorbFlipValue::VFVTwo ] = 0;
            countMap[ VoltorbFlipGrid::VoltorbFlipValue::VFVThree ] = 0;
        }
    }
}

VoltorbFlipGridSolver::VoltorbFlipClue VoltorbFlipGridSolver::getClueFromLine( const VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipGrid::VoltorbFlipValue >& line )
{
    int sum = 0;
    int zeroes = 0;

    for ( const auto& value : line )
    {
        if ( value < NumberOfValues )
        {
            sum += value;
            if ( value == VoltorbFlipGrid::VoltorbFlipValue::VFVZero )
            {
                zeroes++;
            }
        }
    }

    return { sum, zeroes };
}

void VoltorbFlipGridSolver::solve()
{
    if ( !m_solved )
    {
        solveRecursively( m_grid );
        m_solved = true;

        // Verify that the number of solutions is consistent.
        {
            for ( const auto& countRow : m_valueCountGrid )
            {
                for ( const auto& countMap : countRow )
                {
                    int totalNumberOfSolutions = 0;
                    for ( const auto& possibility : countMap )
                    {
                        totalNumberOfSolutions += possibility.second;
                    }
                    assert( totalNumberOfSolutions == m_numberOfSolutions && "Number of solutions don't match!" );
                }
            }
        }
    }
}

void VoltorbFlipGridSolver::solveRecursively( const VoltorbFlipGrid& testState )
{
    // Find out which row we should work with.
    int unfilledRowIndex = VoltorbFlipGridSize;
    bool isGridFilled = isGridCorrectlyFilled( testState, unfilledRowIndex );

    if ( isGridFilled )
    {
        // Base case, grid is correctly filled: Fill in our solution.
        for ( int r=0; r<VoltorbFlipGridSize; r++ )
        {
            for ( int c=0; c<VoltorbFlipGridSize; c++ )
            {
                auto possibleEntryValue = testState.getEntryValue( r, c );
                m_valueCountGrid[ r ][ c ][ possibleEntryValue ]++;
            }
        }
        
        m_numberOfSolutions++;
    }
    else
    {
        if ( unfilledRowIndex == VoltorbFlipGridSize )
        {
            // Another base case, grid is completely but not correctly filled: Just return.
            return;
        }
        else
        {
            // Recursion. Populate the first incomplete row and recurse.
            // Try all rows that match this row's clue.
            for ( const auto& referenceRow : m_referenceClues.at( m_rowClues[ unfilledRowIndex ] ) )
            {
                // Fill in the reference row if it's plausible for the unfilled row.
                if ( verifyLineIsPlausible( referenceRow, m_grid.getRow( unfilledRowIndex ) ) )
                {
                    auto nextState = testState;
                    nextState.setRow( unfilledRowIndex, referenceRow );

                    // Only proceed if columns are also plausible for the reference row.
                    if ( verifyColumnsArePlausible( nextState ) )
                    {
                        // Reference row is plausible
                        solveRecursively( nextState );
                    }
                }
            } // Loop over all reference rows corresponding to this row's clue
        }
    }
}

bool VoltorbFlipGridSolver::isGridCorrectlyFilled( const VoltorbFlipGrid& testState, int& firstUnfilledRow ) const
{
    firstUnfilledRow = VoltorbFlipGridSize;

    // Check rows.
    for ( int r=0; r<VoltorbFlipGridSize; r++ )
    {
        auto clue = getClueFromLine( testState.getRow( r ) );
        if ( clue != m_rowClues[ r ] )
        {
            firstUnfilledRow = r;
            return false;
        }
    }

    // Check columns.
    for ( int c=0; c<VoltorbFlipGridSize; c++ )
    {
        auto clue = getClueFromLine( testState.getColumn( c ) );
        if ( clue != m_columnClues[ c ] )
        {
            return false;
        }
    }

    return true;
}

bool VoltorbFlipGridSolver::verifyLineIsPlausible( const VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipGrid::VoltorbFlipValue >& referenceLine, const VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipGrid::VoltorbFlipValue >& testLine ) const
{
    for ( int i=0; i<VoltorbFlipGridSize; i++ )
    {
        switch ( testLine[ i ] )
        {
            // Test value is unknown.
        case VoltorbFlipGrid::VoltorbFlipValue::VFVUnknown:
            break;

            // Test value is ZeroOrOne, so reference should be zero or one.
        case VoltorbFlipGrid::VoltorbFlipValue::VFVZeroOrOne:
            if ( ( referenceLine[ i ] != VoltorbFlipGrid::VoltorbFlipValue::VFVZero ) &&
                ( referenceLine[ i ] != VoltorbFlipGrid::VoltorbFlipValue::VFVOne ) )
            {
                return false;
            }
            break;

        default:
            if ( testLine[ i ] != referenceLine[ i ] )
            {
                return false;
            }
            break;
        }
    }

    return true;
}

bool VoltorbFlipGridSolver::verifyColumnsArePlausible( const VoltorbFlipGrid& testState ) const
{
    for ( int c=0; c<VoltorbFlipGridSize; c++ )
    {
        auto column = testState.getColumn( c );
        bool columnIsPossible = false;
        for ( const auto& referenceColumn : m_referenceClues.at( m_columnClues[ c ] ) )
        {
            if ( verifyLineIsPlausible( referenceColumn, column ) )
            {
                columnIsPossible = true;
                break;
            }
        }

        if ( !columnIsPossible )
        {
            return false;
        }
    }

    return true;
}
