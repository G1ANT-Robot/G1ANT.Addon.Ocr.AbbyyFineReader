# abbyycell

This structure stores information about the location and content of a table cell and has the following fields:

| Field      | Type    | Description                                                  |
| ---------- | ------- | ------------------------------------------------------------ |
| `top`      | integer | Y coordinate of the top border of a surrounding rectangle (a minimal rectangle containing all the characters of the line) |
| `bottom`   | integer | Y coordinate of the bottom border of a surrounding rectangle (a minimal rectangle containing all the characters of the line) |
| `left`     | integer | X coordinate of the left border of a surrounding rectangle (a minimal rectangle containing all the characters of the line) |
| `right`    | integer | X coordinate of the right border of a surrounding rectangle (a minimal rectangle containing all the characters of the line) |
| `baseline` | integer | Distance from the line on which the characters are located to the top edge of the page |
| `text`     | text    | Content of a cell                                            |

