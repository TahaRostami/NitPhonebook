   M S B A M L   `   `   `  ���� H  DNit.Phonebook, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Nit.PhonebookNit.Phonebook  %    Nit.Phonebook.UcWindowRowEditor   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -localclr-namespace:Nit.Phonebook     F BWpfControls, Version=1.0.0.8, Culture=neutral, PublicKeyToken=null!wpfhttp://wpfcontrols.com/  V RMaterialDesignThemes.Wpf, Version=2.4.0.1044, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes     MaterialDesignBody#���   $��Regular"�$��13(�&   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�   TextRenderingMode$
 Auto=�  MaterialDesignPaper#���    MaterialDesignFont#���  $	��450��$	��768��  PrimaryHueMidBrush#���  $��1q�.��� $��RightToLeft=� � RowDefinitions �� $��50 ��� $
��Auto ��� $
��Auto ��� $
��Auto � � ColumnDefinitions �� �� .+��� -    ��Name$ btnCancelAndClose��$y�	Collapsed=�  MaterialDesignFlatButton#��[ $��2�� K� ZIndex$ 10��$��50��$��35��$
��Left=�$	��Top=���0  *MaterialDesignThemes.Wpf.PackIconExtension     Kind$ Close=�`� -    `�Name$ toolBarTray��$��2��$��0��.�c�   MaterialDesignToolBar#���  }�.  .@��� -   $ btnOk��$
��Left=�#��[ $	��100��$��Bottom=�.��
تایید�� -   $ btnClose��$��Right=�#��[ $	��100��.��
بستن�� -    ��Name$ chkRemoveUnused��	 d� 	IsChecked$	 FalseY�.��ECحذف از جداول اصلی در صورت نبودن ارجاع�� -   $ chkAutoSuggestionMode��$
	 TrueY�.��FDپشینهاد گزینه های منطبق در هنگام تایپ`� -   $ toolBarTrayProgress��$��2��$��0��$y�	Collapsed=�.�c� #���  }�.  .@�#  MaterialDesignThemes.Wpf.Card  
   UniformCornerRadius$
 14Q�$��4q�.���  % !MaterialDesignCircularProgressBar#��[  � Value$ 50Q� � IsIndeterminate . -  'WpfControls.Editors.AutoCompleteTextBox  -     Name$ txtAutoEmployee��$��10q�$	��Top=�$��1��$��0��$��25��   	Watermark$ نام��   IconPlacement$
 Left=�   IconVisibility$ Visible=�   SelectedItem ��  �� Path$ SelectedPath� �� Mode$ TwoWay=�   LoadingContent �� $��
Loading...��$��5q�$��14(�   Icon �� -	   $ btnInsertEmployeeToRow��$��3q�$
��Auto��$
��Auto��$��Center=�$��Center=���  $ Pencil=�&   System.Windows.Controls.DataGrid  -
     Name$ dataGridEmployee��$	��350��$	��250��$��15q�   CanUserAddRows .  $��Center=�$��Center=�$��2��$��0��� $��1��$��1��$��3q�.+��� -   $ chkIsInternalPhoneNumber��$��10(�$
	 TrueY�.��
داخلی  -   $ txtAutoPhone��$	��290��$��10q�$	��Top=�$��25��$ شماره تلفن��$
 Left=�$ Visible=� �� $ SelectedPath�$ TwoWay=� �� $��
Loading...��$��5q�$��14(� �� -   $ btnInsertPhoneToRow��$��3q�$
��Auto��$
��Auto��$��Center=�$��Center=���  $ Pencil=�  -   $ dataGridPhone��$	��350��$	��250��$��15q� .  $��Center=�$��Center=�$��2��$��1��