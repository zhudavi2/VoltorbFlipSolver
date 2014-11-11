//
//  VoltorbFlipGrid.h
//
//  Copyright (c) 2014 David Zhu. All rights reserved.
//

#ifndef __VOLTORBFLIPGRID_H__
#define __VOLTORBFLIPGRID_H__

#include <array>
#include <vector>
#include <map>

// Causes duplicate definition issues if declared inside the class
// Must be declared in the .h so that VoltorbFlipLine may be declared
static const int GridSize = 5;

class VoltorbFlipGrid
{
public:
    template< class T >
    using VoltorbFlipLine = std::array< T, GridSize >;

    typedef std::pair< int, int > VoltorbFlipClue;

    VoltorbFlipGrid( const VoltorbFlipLine< VoltorbFlipClue >& rowClues, const VoltorbFlipLine< VoltorbFlipClue >& columnClues );

    inline VoltorbFlipClue getRowClue( int rowIndex ) const { return m_rowClues.at( rowIndex ); }
    inline VoltorbFlipClue getColumnClue( int columnIndex ) const { return m_columnClues.at( columnIndex ); }

    int& operator()( int rowIndex, int columnIndex ) { return m_gridRepresentation.at( rowIndex ).at( columnIndex ); }
    int operator()( int rowIndex, int columnIndex ) const { return m_gridRepresentation.at( rowIndex ).at( columnIndex ); }

    std::vector< VoltorbFlipGrid > solve() const;

    static const int ZeroValue;
    static const int OneValue;
    static const int TwoValue;
    static const int ThreeValue;
    static const int CouldBeZeroOrOneValue;
    static const int UnknownValue;

private:
    static int getFirstUnfilledRowOfGrid( const VoltorbFlipGrid& testState );

    static VoltorbFlipLine< int > getColumnOfGrid( const VoltorbFlipGrid& testState, int columnIndex );

    static bool verifyLine( const VoltorbFlipLine< int >& referenceLine, const VoltorbFlipLine< int >& testLine, bool strongMatch = false );

    bool solveRecursively( const VoltorbFlipGrid& testState, std::vector< VoltorbFlipGrid >& solutions ) const;

    bool verifyGrid( const VoltorbFlipGrid& testState, bool strongMatch = false ) const;

    static const std::map< VoltorbFlipClue, std::vector< VoltorbFlipLine< int > > > ReferenceClues;

    const VoltorbFlipLine< VoltorbFlipClue > m_rowClues;
    const VoltorbFlipLine< VoltorbFlipClue > m_columnClues;

    VoltorbFlipLine< VoltorbFlipLine< int > > m_gridRepresentation;
};

#endif // __VOLTORBFLIPGRID_H__
