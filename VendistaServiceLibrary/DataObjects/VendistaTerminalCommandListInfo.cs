using System;

namespace VendistaServiceLibrary.DataObjects;

internal record VendistaTerminalCommandListInfo(bool success, int page_number, int items_per_page, int items_count, VendistaTerminalCommandInfo[] items);
