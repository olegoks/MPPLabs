   M S B A M L   `   `   `  ����>  :wpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:AssemblyBrowser.WpfAssemblyBrowser.Wpf  5   	       wpf.MainWindow   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:AssemblyBrowser.Wpf     5   	     � Title$  
MainWindow��5   	   $	��517��6   $	��800��6)    � 	Resources 5	         TreeViewItemTemplate��%2   &	        �� 5
   
   .R�5      � 5      5       � DataContext 5      #   AssemblyBrowser.Wpf.ViewModel  5   
   .��5      �  � RowDefinitions 5   
   �� 5      $	��29* �6   �� 5      $
��378* �6   �� 5      $	��27* �6   .+�5   
   o� $��28��6   $��Center=�6   
����������	#FF323232.@�5      m� $
��File��6   .@�5      m� $
��Open��6    m� Command 6)   �� *OpenFileCommand+5      5      �� 5   
   $��	0,391,0,2q�6   $��1��6'   $��2��64   .@�5      �� $��35��6   $��Bottom=�6(   ���	#FF2C2C2C.��5      �� $
��Wrap=�68   
���������6   �� *
OpenedFile+6L   5      5       R� 5!   
   ���	#FF444444$��1��6M   $��0,0,0,8q�6Z   ��6   �� *
Namespaces+��5"      �� 5#       �� ItemsSource 6,   �� *Nodes+.R�5$      ��  �� Padding$
 0, 3q�6    �� 60   ���#dcdcaa�64   �� *
Optional+ �� UpdateSourceTrigger$ PropertyChanged=�6s   �� 6�   $� ��6�   �� 6�   ���#89d6a2�6�   �� *
NodeType+$ PropertyChanged=�6�   �� 6�   $� ��6�   �� 6  ���#569cd6�6  �� *AccessModifier+$ PropertyChanged=�6K  �� 6b  $� ��6f  �� 6q  ���#569cd6�6u  �� *TypeModifier+$ PropertyChanged=�6�  �� 6�  $� ��6�  �� 6�  ���#569cd6�6�  �� *	ClassType+$ PropertyChanged=�6"  �� 69  $� ��6=  �� 6H  ���#4ec9b0�6L  �� *Type+$ PropertyChanged=�6�  �� 6�  $� ��6�  �� 6�  ���#4ec9b0�6�  �� *
ReturnType+$ PropertyChanged=�6�  �� 6	  $� ��6  �� 6  ���#dcdcdc�6  �� *Name+$ PropertyChanged=�6W  6o  5%      5'      5(      5)      