--定义一些全局的工具函数

local _utility ={
    
    -- lastStringOf("123/456", "/") 返回456
    lastStringOf = function( path, mark)
        for i = 0, string.len(path) do
            local idx = string.len(path) - i
            if idx > 0 then
                if string.sub(path, idx, idx) == mark then
                    return string.sub(path, idx + 1)
                end
            end
        end
        return nil        
    end,

    Split = function (szFullString, szSeparator)  
        local nFindStartIndex = 1  
        local nSplitIndex = 1  
        local nSplitArray = {}  
        while true do  
           local nFindLastIndex = string.find(szFullString, szSeparator, nFindStartIndex)  
           if not nFindLastIndex then  
            nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, string.len(szFullString))  
            break  
           end  
           nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, nFindLastIndex - 1)  
           nFindStartIndex = nFindLastIndex + string.len(szSeparator)  
           nSplitIndex = nSplitIndex + 1  
        end  
        return nSplitArray  
    end,  
    
}

return _utility