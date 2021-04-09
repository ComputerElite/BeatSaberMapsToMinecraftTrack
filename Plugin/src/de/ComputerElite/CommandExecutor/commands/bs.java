package de.ComputerElite.CommandExecutor.commands;

import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;

import org.bukkit.Bukkit;
import org.bukkit.command.Command;
import org.bukkit.command.CommandExecutor;
import org.bukkit.command.CommandSender;
import org.bukkit.entity.Player;

public class bs implements CommandExecutor {
	@Override
	public boolean onCommand(CommandSender sender, Command command, String label, String[] arg) {
		Player Player = ((Player) sender).getPlayer();
		
		if(sender instanceof Player) {
			if(arg.length > 0) {
				Path p = Paths.get(String.join(" ", arg));
				try {
					@SuppressWarnings("rawtypes")
					ArrayList lines = (ArrayList) Files.readAllLines(p, StandardCharsets.UTF_8);
					for(int i = 0; i < lines.size(); i++) {
						Bukkit.getServer().dispatchCommand(Bukkit.getServer().getConsoleSender(), (String)lines.get(i));
					}
					Player.sendMessage("§e[CommandExecutor] §bExecuted §e" + lines.size() + " §bcommands");
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		}
		return false;
	}
}
