import pandas as pd
import matplotlib.pyplot as plt

df = pd.read_csv("new-raw-analytics.csv")
df['Timestamp'] = pd.to_datetime(df['Timestamp'])

# Filter for playerDied events and time
df_deaths = df[df['eventType'] == 'playerDied'].sort_values(['sessionID', 'Timestamp'])
df_deaths['time_diff'] = df_deaths.groupby('sessionID')['Timestamp'].diff().dt.total_seconds()

# Consider only the first death in a 10-second window as a valid death to get rid of duplicates or glitches
df_deaths['valid_death'] = (df_deaths['time_diff'] > 1.5) | (df_deaths['time_diff'].isna())
deaths_per_level = df_deaths[df_deaths['valid_death']].groupby('currentLevel').size()
session_counts = df.groupby('currentLevel')['sessionID'].nunique()
average_deaths_per_player = deaths_per_level / session_counts

df_completed = df[df['eventType'] == 'levelCompleted'].groupby('currentLevel')['sessionID'].nunique()
completion_rate = (df_completed / session_counts * 100).fillna(0)

# Plotting
fig, ax1 = plt.subplots(figsize=(10, 6))
color = 'tab:red'
ax1.set_xlabel('Level')
ax1.set_ylabel('Death Count', color=color)
ax1.bar(average_deaths_per_player.index, average_deaths_per_player.values, color=color)
ax1.tick_params(axis='y', labelcolor=color)

ax2 = ax1.twinx()
color = 'tab:blue'
ax2.set_ylabel('Completion Rate (%)', color=color)
ax2.plot(completion_rate.index, completion_rate.values, color=color, marker='o', linestyle='-')
ax2.tick_params(axis='y', labelcolor=color)

plt.title('Summed Deaths and Completion Rates by Level')
fig.tight_layout()  # Adjust layout to prevent overlap
plt.grid(True)
plt.show()
