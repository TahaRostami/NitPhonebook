   M S B A M L   `   `   `  ���� H  DNit.Phonebook, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Nit.PhonebookNit.Phonebook  !    Nit.Phonebook.UcCategoryBar   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -localclr-namespace:Nit.Phonebook     V RMaterialDesignThemes.Wpf, Version=2.4.0.1044, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes     MaterialDesignBody#���   $��Regular"�$��13(�&   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�   TextRenderingMode$
 Auto=�  MaterialDesignFont#���  
������ $��RightToLeft=�.��� -    �Name$ grid1��.+��� $��0q� � ContextMenu �� .@�m� -   $��اجرای دوباره��m� -   $��
اصلاح�����  � ColumnDefinitions �� �� .+�'  !MaterialDesignThemes.Wpf.PopupBox  -     Name$ 
popupAlter��
�������$��	30,0,30,0q�$��Center=�$��Right=�$��1��   	StaysOpen . {�.    PopupContent. � $	��300��$��8,8,8,8q� � 	Resources 	 �� 
TargetType  MaterialDesignCheckBox
 �� BasedOn �� Property �� Value  MaterialDesignTextBox)  #MaterialDesignThemes.Wpf.HintAssist��%�   '��       7  '�� .     7  �� "	 ��8
   .���  � ��$ 8,8,10,8q��� "	 ��8
   .���  �@ 
IsFloating . ��  � ��$ 8,8,8,8q� �� $��* � � RowDefinitions �� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� $��0��$��0��   MaterialDesignTitleTextBlock#��[ $��8,8,8,16q�#!اصلاح دسته بندی ها�� -    ��Name$ txtTitle��$��0��$��1��   Hint$" عنوان دسته بندی��$�� ���� -   $ txtId��$y�	Collapsed=�$��0��$��1��$�� ���� -   $ txtParentId��$y�	Collapsed=�$��0��$��1��$�� ��� $��0��$��2��.+��� -    ��Name$ 	btnInsert�� � ToolTip$ 
ایجاد����0  *MaterialDesignThemes.Wpf.PackIconExtension     Kind$ Pencil=��� -	   $ 	btnUpdate��$ بروزرسانی����  $ Update=��� -
   $ 	btnDelete��$ حذف����  $ Delete=��� -    ��Name$ chkAlterMode�� d� 	IsChecked$ FalseY�$��مد اصلاح���� -   $ chkSync��$ FalseY�$
��sync���� -    ��Name$ txtBlockMsg��$��0��$��3��$��Center=�
���  ���� -   $��
btnExecute��
�������$��1��$��	75,0,75,0q�$��Right=�$ اجرای دوباره��$��10q���  $ Reload=�#  MaterialDesignThemes.Wpf.Card  -     Name$ progressBar��$��1��$��5,0,5,0q�$��Right=�   UniformCornerRadius$ 14Q�$��4q�$y�	Collapsed=�.���  % !MaterialDesignCircularProgressBar#��[  � Value$ 50Q� � IsIndeterminate . �� 
�������$��Medium"�$��Center=�$
��Left=�$��0��$��دسته بندی ها�� �� Padding$ 10q�.��R� -   $��treeCategory������  �� DataType" m� �� ItemsSource �� *Items+.R��� ���� *Category.Title+