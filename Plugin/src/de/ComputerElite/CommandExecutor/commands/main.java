package de.ComputerElite.CommandExecutor.commands;

import org.bukkit.plugin.java.JavaPlugin;

public class main extends JavaPlugin {
	public void onEnable() {
		System.out.println("[CommandExecutor] activated");
		getCommand("bs").setExecutor(new bs());
		
	}
	
	public void onDisable() {
		
		System.out.println("[CommandExecutor] deactivated");
	}
}
