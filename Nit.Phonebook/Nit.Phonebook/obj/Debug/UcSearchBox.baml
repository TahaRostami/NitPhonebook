   M S B A M L   `   `   `  ����H  DNit.Phonebook, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Nit.PhonebookNit.Phonebook  5          Nit.Phonebook.UcSearchBox   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -localclr-namespace:Nit.Phonebook     5      F BWpfControls, Version=1.0.0.8, Culture=neutral, PublicKeyToken=null!wpfhttp://wpfcontrols.com/  5      V RMaterialDesignThemes.Wpf, Version=2.4.0.1044, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	         MaterialDesignBody#���   5
      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignFont#���  5      
������ $��RightToLeft=�5   7   .��5      � .+�5   
   �� ��5      �� 5      .+�5      �  � ColumnDefinitions 5      �� 5      �� 5      .+�5      �� 
�������$��Medium"�6>   $��Center=�6S   $��Right=�6o   $��1��6�   $��نوار جستجو��6�    �� Padding$ 10q�6�   6�   � 5      .+�5      �� -   5         ��Name$ 	btnDelete��5   %   .��5   "   '  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.4.0.1044, Culture=neutral, PublicKeyToken=null]]   Kind$ DeleteCircle=�6:   $��20��6N   $��20��6Y   5      � 5       $��6q�6(   .+�5!   "   	� -   5         	�Name$ 	searchEmp��5!   .    	� 	GroupName$ 
SearchType��6A    d� 	IsChecked$
 TrueY�6X   
�������  SecondaryAccentBrush#���  6|   .��6�   
افراد5#   #   	� 5$   "   -   5        $ seacrhPh��5$   .   $ 
SearchType��6@   #���  6W   
�������.��6�   
تلفن5&   #   5'      5(      5)      5*      .��5,      � 	 � RowDefinitions	 5-      �� 5.      $
��Auto �6$   �� 5/       51      �� 52      $��35 �6'   �� 53      .+�55      -  'WpfControls.Editors.AutoCompleteTextBox  -   5        
  Name$
 	txtSearch��55   7   $��10q�6+   $	��Top=�6J   $��0��6b   $��2��6o   $��30��56   "      	Watermark$ 
جستجو��57   "      IconPlacement$
 Left=�58   "      IconVisibility$ Visible=�59   "      SelectedItem 5:      �� 5;       �� Path$ SelectedPath�6"    �� Mode$ TwoWay=�66   5<         LoadingContent 5>      �� 5?      $��
Loading...��6$   $��5q�5@      $��14(�5A         Icon 5C      �� 5D      -   5        $ 	btnSearch��5D   !   
������ 
���   �$��5q�6�   ��6L   0  *MaterialDesignThemes.Wpf.PackIconExtension     Kind$ DatabaseSearch=�6�   5F      �� 5G      -   5         ��Name$ lstSearchResults��5G   H   $��0,8,0,0q�6   $��1��6+   $��1��68   5I      5K      5L      #  MaterialDesignThemes.Wpf.Card  5M   
   -   5          Name$ progressBar��5M   1    K� ZIndex$	 900��6   $��5,0,5,0q�6F   $��Center=�6W   $��Center=�6t      UniformCornerRadius$ 14Q�6�   $��4q�6�   $y�	Collapsed=�6�   .��5N      �  % !MaterialDesignCircularProgressBar#��[ 6    � Value$ 50Q�6U    � IsIndeterminate . 6`   5O      5P      5Q      