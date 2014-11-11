//
//  VoltorbFlipProbabilities.h
//
//  Copyright (c) 2014 David Zhu. All rights reserved.
//

#include "VoltorbFlipProbabilities.h"

VoltorbFlipProbabilities::VoltorbFlipProbabilities( const std::vector< VoltorbFlipGrid >& possibilities ) :
m_totalSolutions( possibilities.size() )
{
    for ( auto& row : m_valueCounts )
    {
        for ( auto& cell : row )
        {
            for ( int& count : cell )
            {
                count = 0;
            }
        }
    }

    for ( auto& possibleGrid : possibilities )
    {
        for ( int r=0; r<GridSize; r++ )
        {
            for ( int c=0; c<GridSize; c++ )
            {
                m_valueCounts[ r ][ c ][ possibleGrid( r, c ) ]++;
            }
        }
    }
}

float VoltorbFlipProbabilities::getZeroProbabilityOfCell( int row, int column ) const
{
    return static_cast< float >( m_valueCounts[ row ][ column ][ 0 ] ) / static_cast< float >( m_totalSolutions );
}

float VoltorbFlipProbabilities::getZeroOrOneProbabilityOfCell( int row, int column ) const
{
    return this->getZeroProbabilityOfCell( row, column ) +
        ( static_cast< float >( m_valueCounts[ row ][ column ][ 1 ] ) / static_cast< float >( m_totalSolutions ) );
}
