   M S B A M L   `   `   `  ����H  DNit.Phonebook, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Nit.PhonebookNit.Phonebook  5          Nit.Phonebook.UcRowEditor   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -localclr-namespace:Nit.Phonebook     5      V RMaterialDesignThemes.Wpf, Version=2.4.0.1044, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5         MaterialDesignBody#���   5	      $��Regular"�5
      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignFont#���  5      
������ $��RightToLeft=�5   8    � 	Resources 5      *  $System.Windows.Controls.DataGridCell �� 
TargetType  MaterialDesignDataGridCell �� BasedOn �� Property �� Value��%m   '        7  �� 5   
   "  6   8   .�5      ��  �@��TextAlignment$ Center=�68   5      5      .��5      � .+�5   
   �� -   5         ��Name$ 	groupBox1��5       � ContextMenu 5      �� 5      .@�5      m� -   5        $��اجرای دوباره��5      65   5      ��5      �� 5      .+�5      � 	 � ColumnDefinitions	 5       �� 5!      �� 5"      .+�5$      �� -   5        $��
btnExecute��5$   M   $
��Auto��6!   
�������$��1��6z   $��Right=�6�   
 � ToolTip$
 اجرای دوباره��6�   $��10q�6�   ��6�   0  *MaterialDesignThemes.Wpf.PackIconExtension     Kind$ Reload=�6�   �� 5%      -   5        $��	btnInsert��5%   C   $
��Auto��6!   $��	40,0,40,0q�60   
�������$��1��6�   $��Right=�6�   $
 افزودن��6�   $��10q�6�   ��6�     $ Pencil=�6�   �� 5&      -   5        $��	btnUpdate��5&   C   $
��Auto��6!   $��	80,0,80,0q�60   
�������$��1��6�   $��Right=�6�   $
 ویرایش��6�   $��10q�6�   ��6�     $ Update=�6�   �� 5'      
�������$��Medium"�6>   $��Center=�6S   $
��Left=�6o   $��0��6�   $��لیست سطر ها��6�    �� Padding$ 10q�6�   6�   5(      5)      .��5+      &   System.Windows.Controls.DataGrid  -   5          Name$ 	dataGrid1��5+   &   $��12(�6   $��0 8 0 0q�69      CanUserAddRows .     CanUserDeleteRows .    SelectionMode$ Single=�6{   5-      5.      #  MaterialDesignThemes.Wpf.Card  5/   
   -   5          Name$ progressBar��5/   1    K� ZIndex$	 900��6   $��1��6F   $��5,0,5,0q�6V   $��Center=�6g   $��Center=�6�      UniformCornerRadius$ 14Q�6�   $��4q�6�   $y�	Collapsed=�6�   .��50      �  % !MaterialDesignCircularProgressBar#��[ 6    � Value$ 50Q�6U    � IsIndeterminate . 6`   51      57      58      