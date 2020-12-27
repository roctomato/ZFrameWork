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

}

return _utility