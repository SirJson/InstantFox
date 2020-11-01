# InstantFox

A utility written in C# that downloads the latest Firefox, set up a portable instance and installs desired user extensions.

You can keep that new instance or use delete it after you close the browser.

This tool is written for the Windows Sandbox to install and setup Firefox automatically.

## Notes

- To keep it simple and focused it will always download
    - The latest Firefox
    - The en-US Version
    - For Windows 64-bit

There is no reason though you could easily change those parameters.

## User Extensions

If you want to add your themes and extensions put a folder "exts" next to it and put the XPIs of your extensions into it.
Make sure to rename them after their [add-on ids](https://extensionworkshop.com/documentation/develop/extensions-and-the-add-on-id/) in order to auto-install them on startup.