   M S B A M L   `   `   `  ����H  DNit.Phonebook, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Nit.PhonebookNit.Phonebook  5      +    %Nit.Phonebook.PageMainTableInfoEditor   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -localclr-namespace:Nit.Phonebook     5      V RMaterialDesignThemes.Wpf, Version=2.4.0.1044, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5         MaterialDesignBody#���   5	   	   $��Regular"�5
   	   $��13(�5   	   &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5   	      TextRenderingMode$
 Auto=�5   	     MaterialDesignPaper N� 
Background# �  5   	     MaterialDesignFont N� 
FontFamily# �  5   	    N� Title$ PageMainTableInfoEditor��5      .��5      � .+�5   
   ]� -   5         ]�Name$ 
dockPanel1��5      .+�5      � -   5         �Name$ 
wrapPanel2��5      $��Right=�6-   $��38��6D     PrimaryHueMidBrush#���  6O   .+�5      �� -   5         ��Name$ btnLogin��5       & "MaterialDesignFloatingActionButton#��[ 6+   $��2,80,0,0q�6g   $��35��6z   $��35��6�   .��5      '  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.4.0.1044, Culture=neutral, PublicKeyToken=null]]   Kind$ Login=�6.   $��22��6<   $��22��6H   5      �� 5      -   5        $ btnTheme��5       . *MaterialDesignFloatingActionMiniDarkButton#��[ 6B   $��2,15,0,0q�6�   $��35��6�   $��35��6�   .��5        $ Settings=�6.   $��22��6?   $��22��6K   5      5      !   Nit.Phonebook.UcCategoryBar  5      -   5        	  Name$	 ucCat��5   "   $��Right=�61   $	��435��6H   � 5      -   5        $ 
wrapPanel1��5      $
��Left=�6-   $��38��6C   #���  6N   .+�5      �� -   5        $ btnEmployeeLeftMenu��5      $
��0,80q�66   $��افـــــراد��6E   
 � LayoutTransform
 5       �� 5!       �� Angle$ 90Q�5"      5$      �� 5%      -   5        $ btnPhoneLeftMenu��5%      $��0,-70q�63   $��شماره تلفن ها��6C   
 5&      �� 5'      $ 90Q�5(      5*      �� 5+      -	   5        $ btnSearchBoxLeftMenu��5+      $
��0,80q�67   $��نوار جستجو��6F   
 5,      �� 5-      $ 90Q�5.      50      �� 51      -
   5        $ btnReportLeftMenu��51      $��0,-60q�64   $��تهیه گزارش��6D   ���	#FF6FA663
 52      �� 53      $ 90Q�54      56      57         Nit.Phonebook.UcRowEditor  58      -   5          Name$ ucRowEditor��58       5;      � 5=   
   -   5         �Name$ GridBackgroundPage��5=       K� ZIndex$
 1000��6+   
���   � J� Opacity$	 0.5Q�6S   $y�	Collapsed=�6a   5?         Nit.Phonebook.UcStartup  5A   
   -   5          Name$ ucLogin��5A      $��Center=�6,   $��Center=�6I   $
 1002��6e      Nit.Phonebook.UcTheme  5C   
   -   5          Name$ ucThemeSettings��5C      $
 1002��61   $��Center=�6E   $��Center=�6`   $y�	Collapsed=�6}   %   Nit.Phonebook.UcWindowRowEditor  5E   
   -   5          Name$ ucWindowRowEditor��5E   6   %	   Nit.Phonebook.UcWindowRowEditor 	  	UcClosing$! ucWindowRowEditor_UcClosing��6�   $
 1002��6"   $	��450��6Q   $	��768��6^   $��Center=�6j   $��Center=�6�   $y�	Collapsed=�6�   6�   5F      5G      