   M S B A M L   `   `   `  ���� H  DNit.Phonebook, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Nit.PhonebookNit.Phonebook      Nit.Phonebook.UcSearchBox   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -localclr-namespace:Nit.Phonebook     F BWpfControls, Version=1.0.0.8, Culture=neutral, PublicKeyToken=null!wpfhttp://wpfcontrols.com/  V RMaterialDesignThemes.Wpf, Version=2.4.0.1044, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes     MaterialDesignBody#���   $��Regular"�$��13(�&   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�   TextRenderingMode$
 Auto=�  MaterialDesignFont#���  
������ $��RightToLeft=�.��� .+��� ���� .+��  � ColumnDefinitions �� �� .+��� 
�������$��Medium"�$��Center=�$��Right=�$��1��$��نوار جستجو�� �� Padding$ 10q�� .+��� -    ��Name$ 	btnDelete��.��'  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.4.0.1044, Culture=neutral, PublicKeyToken=null]]   Kind$ DeleteCircle=�$��20��$��20��� $��6q�.+�	� -    	�Name$ 	searchEmp�� 	� 	GroupName$ 
SearchType�� d� 	IsChecked$
 TrueY�
�������  SecondaryAccentBrush#���  .��
افراد	� -   $ seacrhPh��$ 
SearchType��#���  
�������.��
تلفن.��� 	 � RowDefinitions	 �� $
��Auto ���  �� $��35 ��� .+�-  'WpfControls.Editors.AutoCompleteTextBox  -   
  Name$
 	txtSearch��$��10q�$	��Top=�$��0��$��2��$��30��   	Watermark$ 
جستجو��   IconPlacement$
 Left=�   IconVisibility$ Visible=�   SelectedItem ��  �� Path$ SelectedPath� �� Mode$ TwoWay=�   LoadingContent �� $��
Loading...��$��5q�$��14(�   Icon �� -   $ 	btnSearch��
������ 
���   �$��5q���0  *MaterialDesignThemes.Wpf.PackIconExtension     Kind$ DatabaseSearch=��� -    ��Name$ lstSearchResults��$��0,8,0,0q�$��1��$��1��#  MaterialDesignThemes.Wpf.Card  -     Name$ progressBar�� K� ZIndex$	 900��$��5,0,5,0q�$��Center=�$��Center=�   UniformCornerRadius$ 14Q�$��4q�$y�	Collapsed=�.���  % !MaterialDesignCircularProgressBar#��[  � Value$ 50Q� � IsIndeterminate . 