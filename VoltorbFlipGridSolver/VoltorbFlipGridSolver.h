//
//  VoltorbFlipGridSolver.h
//
//  Copyright (c) 2014 David Zhu. All rights reserved.
//

#ifndef __VOLTORBFLIPGRIDSOLVER_H__
#define __VOLTORBFLIPGRIDSOLVER_H__

#include "VoltorbFlipGrid.h"
#include <array>
#include <vector>

class VoltorbFlipGridSolver
{
public:
    typedef std::pair< int, int > VoltorbFlipClue;

    static const int NumberOfValues;

    static const std::map< int, VoltorbFlipGrid::VoltorbFlipValue > IntToVoltorbFlipValue;

    static const int UnsolvedGridValueCount;

    VoltorbFlipGridSolver( const VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipClue >& rowClues, const VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipClue >& columnClues );

    inline void setEntryValue( int rowIndex, int columnIndex, VoltorbFlipGrid::VoltorbFlipValue value = VoltorbFlipGrid::VoltorbFlipValue::VFVUnknown )
    { m_grid.setEntryValue( rowIndex, columnIndex, value ); }

    void solve();

    inline int getValueCountOfEntry( int rowIndex, int columnIndex, VoltorbFlipGrid::VoltorbFlipValue value = VoltorbFlipGrid::VoltorbFlipValue::VFVZero ) const
    { return m_solved ? m_valueCountGrid.at( rowIndex ).at( columnIndex ).at( value ) : UnsolvedGridValueCount; }

private:
    static VoltorbFlipClue getClueFromLine( const VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipGrid::VoltorbFlipValue >& line );

    // Returns
    // - true, firstUnfilledRow = VoltorbFlipGridSize if grid is completely and correctly filled
    // - false, firstUnfilledRow = VoltorbFlipGridSize if grid is completely filled but columns are not consistent
    // - false, firstUnfilledRow < VoltorbFlipGridSize if grid is not completely filled
    bool isGridCorrectlyFilled( const VoltorbFlipGrid& testState, int& firstUnfilledRow ) const;

    void solveRecursively( const VoltorbFlipGrid& testState );

    bool verifyLineIsPlausible( const VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipGrid::VoltorbFlipValue >& referenceLine, const VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipGrid::VoltorbFlipValue >& testLine ) const;

    bool verifyColumnsArePlausible( const VoltorbFlipGrid& testState ) const;

    std::map< VoltorbFlipClue, std::vector< VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipGrid::VoltorbFlipValue > > > m_referenceClues;

    const VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipClue > m_rowClues;
    const VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipClue > m_columnClues;

    VoltorbFlipGrid m_grid;

    VoltorbFlipGrid::VoltorbFlipLine< VoltorbFlipGrid::VoltorbFlipLine< std::map< VoltorbFlipGrid::VoltorbFlipValue, int > > > m_valueCountGrid;

    bool m_solved;
    int m_numberOfSolutions;
};

#endif // __VOLTORBFLIPGRIDSOLVER_H__
