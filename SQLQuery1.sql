SELECT 'INSERT INTO StockHistories ([StockHistories].[HistoryDate],[StockHistories].[Open],[StockHistories].[High],[StockHistories].[Low]' +
	', [StockHistories].[Close],[StockHistories].[Volume],[StockHistories].[AdjClose],[StockHistories].[StockSymbolId]) Values (''' + 
  CONVERT(varchar, HistoryDate, 120) 
  + ''', '  + Cast([StockHistories].[Open] as varchar) + ', ' + Cast(High as varchar) + ', ' + Cast(Low as varchar) +',' + 
  Cast([StockHistories].[Close] as varchar) + ', ' + Cast(Volume as varchar)+', ' + Cast(AdjClose as varchar) + ', 16)'  
  
   as text1  FROM StockHistories WHERE StockSymbolId = 2035