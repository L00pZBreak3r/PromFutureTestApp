using System;

namespace VendistaServiceLibrary.DataObjects;

internal record VendistaTerminalLogTable(bool success, int page_number, int items_per_page, int items_count, VendistaTerminalLogTableRow[] items);
