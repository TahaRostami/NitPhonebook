   M S B A M L   `   `   `  ���� H  DNit.Phonebook, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Nit.PhonebookNit.Phonebook  +    %Nit.Phonebook.PageMainTableInfoEditor   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -localclr-namespace:Nit.Phonebook     V RMaterialDesignThemes.Wpf, Version=2.4.0.1044, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes     MaterialDesignBody#���   $��Regular"�$��13(�&   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�   TextRenderingMode$
 Auto=�  MaterialDesignPaper N� 
Background# �    MaterialDesignFont N� 
FontFamily# �   N� Title$ PageMainTableInfoEditor��.��� .+�]� -    ]�Name$ 
dockPanel1��.+�� -    �Name$ 
wrapPanel2��$��Right=�$��38��  PrimaryHueMidBrush#���  .+��� -    ��Name$ btnLogin�� & "MaterialDesignFloatingActionButton#��[ $��2,80,0,0q�$��35��$��35��.��'  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.4.0.1044, Culture=neutral, PublicKeyToken=null]]   Kind$ Login=�$��22��$��22���� -   $ btnTheme�� . *MaterialDesignFloatingActionMiniDarkButton#��[ $��2,15,0,0q�$��35��$��35��.��  $ Settings=�$��22��$��22��!   Nit.Phonebook.UcCategoryBar  -   	  Name$	 ucCat��$��Right=�$	��435��� -   $ 
wrapPanel1��$
��Left=�$��38��#���  .+��� -   $ btnEmployeeLeftMenu��$
��0,80q�$��افـــــراد��
 � LayoutTransform
 ��  �� Angle$ 90Q��� -   $ btnPhoneLeftMenu��$��0,-70q�$��شماره تلفن ها��
 �� $ 90Q��� -	   $ btnSearchBoxLeftMenu��$
��0,80q�$��نوار جستجو��
 �� $ 90Q��� -
   $ btnReportLeftMenu��$��0,-60q�$��تهیه گزارش�����	#FF6FA663
 �� $ 90Q�   Nit.Phonebook.UcRowEditor  -     Name$ ucRowEditor��� -    �Name$ GridBackgroundPage�� K� ZIndex$
 1000��
���   � J� Opacity$	 0.5Q�$y�	Collapsed=�   Nit.Phonebook.UcStartup  -     Name$ ucLogin��$��Center=�$��Center=�$
 1002��   Nit.Phonebook.UcTheme  -     Name$ ucThemeSettings��$
 1002��$��Center=�$��Center=�$y�	Collapsed=�%   Nit.Phonebook.UcWindowRowEditor  -     Name$ ucWindowRowEditor��%	   Nit.Phonebook.UcWindowRowEditor 	  	UcClosing$! ucWindowRowEditor_UcClosing��$
 1002��$	��450��$	��768��$��Center=�$��Center=�$y�	Collapsed=�