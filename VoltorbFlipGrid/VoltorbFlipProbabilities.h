//
//  VoltorbFlipProbabilities.h
//
//  Copyright (c) 2014 David Zhu. All rights reserved.
//

#ifndef __VOLTORBFLIPPROBABILITIES_H__
#define __VOLTORBFLIPPROBABILITIES_H__

#include "VoltorbFlipGrid.h"

static const int NumberOfPossibleValues = 4;

class VoltorbFlipProbabilities
{
public:
    typedef std::array< int, NumberOfPossibleValues > VoltorbFlipValueCounts;

    VoltorbFlipProbabilities( const std::vector< VoltorbFlipGrid >& possibilities );

    inline int getTotalSolutions() const { return m_totalSolutions; }
    inline int getCountOfCell( int row, int column, int number ) const { return m_valueCounts.at( row ).at( column ).at( number ); }

    inline float getZeroProbabilityOfCell( int row, int column ) const;
    inline float getZeroOrOneProbabilityOfCell( int row, int column ) const;
    inline float getTwoOrThreeProbabilityOfCell( int row, int column ) const { return 1.f - this->getZeroProbabilityOfCell( row, column ); }

private:
    int m_totalSolutions;

    VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipGrid::VoltorbFlipLine < VoltorbFlipValueCounts > > m_valueCounts;
};

#endif // __VOLTORBFLIPPROBABILITIES_H__