   M S B A M L   `   `   `  ���� H  DNit.Phonebook, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Nit.PhonebookNit.Phonebook      Nit.Phonebook.UcRowEditor   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -localclr-namespace:Nit.Phonebook     V RMaterialDesignThemes.Wpf, Version=2.4.0.1044, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes     MaterialDesignBody#���   $��Regular"�$��13(�&   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�   TextRenderingMode$
 Auto=�  MaterialDesignFont#���  
������ $��RightToLeft=� � 	Resources *  $System.Windows.Controls.DataGridCell �� 
TargetType  MaterialDesignDataGridCell �� BasedOn �� Property �� Value��%H   '        7  �� "  8   .���  �@��TextAlignment$ Center=�.��� .+��� -    ��Name$ 	groupBox1�� � ContextMenu �� .@�m� -   $��اجرای دوباره������ .+�� 	 � ColumnDefinitions	 �� �� .+��� -   $��
btnExecute��$
��Auto��
�������$��1��$��Right=�
 � ToolTip$
 اجرای دوباره��$��10q���0  *MaterialDesignThemes.Wpf.PackIconExtension     Kind$ Reload=��� -   $��	btnInsert��$
��Auto��$��	40,0,40,0q�
�������$��1��$��Right=�$
 افزودن��$��10q���  $ Pencil=��� -   $��	btnUpdate��$
��Auto��$��	80,0,80,0q�
�������$��1��$��Right=�$
 ویرایش��$��10q���  $ Update=��� 
�������$��Medium"�$��Center=�$
��Left=�$��0��$��لیست سطر ها�� �� Padding$ 10q�.��&   System.Windows.Controls.DataGrid  -     Name$ 	dataGrid1��$��12(�$��0 8 0 0q�   CanUserAddRows .     CanUserDeleteRows .    SelectionMode$ Single=�#  MaterialDesignThemes.Wpf.Card  -     Name$ progressBar�� K� ZIndex$	 900��$��1��$��5,0,5,0q�$��Center=�$��Center=�   UniformCornerRadius$ 14Q�$��4q�$y�	Collapsed=�.���  % !MaterialDesignCircularProgressBar#��[  � Value$ 50Q� � IsIndeterminate . 